using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;
using Webhooks.Core.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core;

public static class WebhookEventBroadcasterOptionsExtensions
{
    public static WebhookBroadcasterOptions UseStrategy<T>(this WebhookBroadcasterOptions options) where T : IBroadcasterStrategy
    {
        options.BroadcasterStrategy = typeof(T);
        return options;
    }
    
    public static WebhookBroadcasterOptions UseStrategy(this WebhookBroadcasterOptions options, Type strategyType)
    {
        if (!strategyType.IsAssignableTo(typeof(IBroadcasterStrategy)))
            throw new ArgumentException($"{nameof(strategyType)} must be assignable from {nameof(IBroadcasterStrategy)}");
        
        options.BroadcasterStrategy = strategyType;
        return options;
    }

    public static WebhookBroadcasterOptions UseDefaultDispatcher(this WebhookBroadcasterOptions options, string dispatcherName)
    {
        if (string.IsNullOrWhiteSpace(dispatcherName))
            throw new ArgumentException("Dispatcher name cannot be empty.", nameof(dispatcherName));

        options.DefaultDispatcher = dispatcherName;
        return options;
    }

    public static WebhookBroadcasterOptions UseQueueCapacity(this WebhookBroadcasterOptions options, int queueCapacity)
    {
        if (queueCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(queueCapacity));

        options.QueueCapacity = queueCapacity;
        return options;
    }

    public static WebhookBroadcasterOptions UseWorkerParallelism(this WebhookBroadcasterOptions options, int workerParallelism)
    {
        if (workerParallelism <= 0)
            throw new ArgumentOutOfRangeException(nameof(workerParallelism));

        options.WorkerParallelism = workerParallelism;
        return options;
    }

    public static WebhookBroadcasterOptions UseOverflowPolicy(this WebhookBroadcasterOptions options, OverflowPolicy overflowPolicy)
    {
        options.OverflowPolicy = overflowPolicy;
        return options;
    }

    public static WebhookBroadcasterOptions UseDeduplication(this WebhookBroadcasterOptions options, bool enabled = true)
    {
        options.DeduplicationEnabled = enabled;
        return options;
    }

    public static WebhookBroadcasterOptions UseRetryAttempts(this WebhookBroadcasterOptions options, int retryAttempts)
    {
        if (retryAttempts <= 0)
            throw new ArgumentOutOfRangeException(nameof(retryAttempts));

        options.RetryAttempts = retryAttempts;
        return options;
    }

    public static WebhookBroadcasterOptions UseSequentialBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<SequentialBroadcasterStrategy>();
    public static WebhookBroadcasterOptions UseParallelTaskBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<ParallelTaskBroadcasterStrategy>();
    public static WebhookBroadcasterOptions UseBackgroundProcessorBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<BackgroundProcessorBroadcasterStrategy>();
}

public class ValidateWebhookSinksOptions : IValidateOptions<WebhookSinksOptions>
{
    private static readonly Regex JsonPathSubsetRegex = new("^\\$([.][A-Za-z_][A-Za-z0-9_]*)+$", RegexOptions.Compiled);

    public ValidateOptionsResult Validate(string? name, WebhookSinksOptions options)
    {
        var sinks = options.Sinks.ToList();

        if (sinks.Count == 0)
        {
            return ValidateOptionsResult.Success;
        }

        var duplicateSinkIds = sinks
            .Where(s => !string.IsNullOrWhiteSpace(s.SinkId))
            .GroupBy(s => s.SinkId, StringComparer.OrdinalIgnoreCase)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToArray();

        if (duplicateSinkIds.Length > 0)
        {
            return ValidateOptionsResult.Fail($"Duplicate sink identifiers: {string.Join(", ", duplicateSinkIds)}");
        }

        foreach (var sink in sinks)
        {
            if (string.IsNullOrWhiteSpace(sink.SinkId))
            {
                return ValidateOptionsResult.Fail("SinkId is required for each sink.");
            }

            if (sink.Destination is null || !sink.Destination.IsAbsoluteUri)
            {
                return ValidateOptionsResult.Fail($"Sink '{sink.SinkId}' must declare an absolute destination URL.");
            }

            if (sink.Subscriptions.Count == 0)
            {
                return ValidateOptionsResult.Fail($"Sink '{sink.SinkId}' must declare at least one subscription.");
            }

            foreach (var subscription in sink.Subscriptions)
            {
                if (subscription.PayloadFilters.Count > 0 && subscription.PayloadMatchingMode is null)
                {
                    return ValidateOptionsResult.Fail(
                        $"Sink '{sink.SinkId}' subscription '{subscription.EventType}' must declare payload matching mode when payload predicates are present.");
                }

                foreach (var payloadFilter in subscription.PayloadFilters)
                {
                    if (!JsonPathSubsetRegex.IsMatch(payloadFilter.Selector))
                    {
                        return ValidateOptionsResult.Fail(
                            $"Sink '{sink.SinkId}' has invalid payload selector '{payloadFilter.Selector}'. Only restricted JsonPath subset is supported.");
                    }
                }
            }
        }

        return ValidateOptionsResult.Success;
    }
}