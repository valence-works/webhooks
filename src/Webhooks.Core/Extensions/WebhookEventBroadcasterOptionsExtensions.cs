using Microsoft.Extensions.Options;
using Webhooks.Core.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core;

/// <summary>
/// Fluent configuration methods for <see cref="WebhookBroadcasterOptions"/>.
/// </summary>
public static class WebhookEventBroadcasterOptionsExtensions
{
    /// <summary>
    /// Sets the broadcaster strategy to the specified type.
    /// </summary>
    public static WebhookBroadcasterOptions UseStrategy<T>(this WebhookBroadcasterOptions options) where T : IBroadcasterStrategy
    {
        options.BroadcasterStrategy = typeof(T);
        return options;
    }
    
    /// <summary>
    /// Sets the broadcaster strategy to the specified type.
    /// </summary>
    public static WebhookBroadcasterOptions UseStrategy(this WebhookBroadcasterOptions options, Type strategyType)
    {
        if (!strategyType.IsAssignableTo(typeof(IBroadcasterStrategy)))
            throw new ArgumentException($"{nameof(strategyType)} must be assignable from {nameof(IBroadcasterStrategy)}");
        
        options.BroadcasterStrategy = strategyType;
        return options;
    }

    /// <summary>
    /// Sets the default dispatcher name used when a sink does not specify one.
    /// </summary>
    public static WebhookBroadcasterOptions UseDefaultDispatcher(this WebhookBroadcasterOptions options, string dispatcherName)
    {
        if (string.IsNullOrWhiteSpace(dispatcherName))
            throw new ArgumentException("Dispatcher name cannot be empty.", nameof(dispatcherName));

        options.DefaultDispatcher = dispatcherName;
        return options;
    }

    /// <summary>
    /// Sets the maximum queue capacity for the background task channel.
    /// </summary>
    public static WebhookBroadcasterOptions UseQueueCapacity(this WebhookBroadcasterOptions options, int queueCapacity)
    {
        if (queueCapacity <= 0)
            throw new ArgumentOutOfRangeException(nameof(queueCapacity));

        options.QueueCapacity = queueCapacity;
        return options;
    }

    /// <summary>
    /// Sets the number of parallel worker tasks for background processing.
    /// </summary>
    public static WebhookBroadcasterOptions UseWorkerParallelism(this WebhookBroadcasterOptions options, int workerParallelism)
    {
        if (workerParallelism <= 0)
            throw new ArgumentOutOfRangeException(nameof(workerParallelism));

        options.WorkerParallelism = workerParallelism;
        return options;
    }

    /// <summary>
    /// Sets the overflow policy when the background queue is full.
    /// </summary>
    public static WebhookBroadcasterOptions UseOverflowPolicy(this WebhookBroadcasterOptions options, OverflowPolicy overflowPolicy)
    {
        options.OverflowPolicy = overflowPolicy;
        return options;
    }

    /// <summary>
    /// Enables or disables event deduplication.
    /// </summary>
    public static WebhookBroadcasterOptions UseDeduplication(this WebhookBroadcasterOptions options, bool enabled = true)
    {
        options.DeduplicationEnabled = enabled;
        return options;
    }

    /// <summary>
    /// Sets the number of retry attempts for failed deliveries.
    /// </summary>
    public static WebhookBroadcasterOptions UseRetryAttempts(this WebhookBroadcasterOptions options, int retryAttempts)
    {
        if (retryAttempts <= 0)
            throw new ArgumentOutOfRangeException(nameof(retryAttempts));

        options.RetryAttempts = retryAttempts;
        return options;
    }

    /// <summary>
    /// Sets the event-type matcher strategy to the specified type.
    /// </summary>
    public static WebhookBroadcasterOptions UseEventTypeMatcher<T>(this WebhookBroadcasterOptions options) where T : IEventTypeMatcherStrategy
    {
        options.EventTypeMatcherStrategy = typeof(T);
        return options;
    }

    /// <summary>
    /// Sets the event-type matcher strategy to the specified type.
    /// </summary>
    public static WebhookBroadcasterOptions UseEventTypeMatcher(this WebhookBroadcasterOptions options, Type matcherType)
    {
        if (!matcherType.IsAssignableTo(typeof(IEventTypeMatcherStrategy)))
            throw new ArgumentException($"{nameof(matcherType)} must be assignable from {nameof(IEventTypeMatcherStrategy)}");

        options.EventTypeMatcherStrategy = matcherType;
        return options;
    }

    /// <summary>Configures the default wildcard-capable event-type matcher.</summary>
    public static WebhookBroadcasterOptions UseWildcardEventTypeMatcher(this WebhookBroadcasterOptions options) => options.UseEventTypeMatcher<WildcardEventTypeMatcherStrategy>();
    /// <summary>Configures exact-only event-type matching (no wildcard support).</summary>
    public static WebhookBroadcasterOptions UseExactEventTypeMatcher(this WebhookBroadcasterOptions options) => options.UseEventTypeMatcher<ExactEventTypeMatcherStrategy>();

    /// <summary>Configures sequential broadcasting.</summary>
    public static WebhookBroadcasterOptions UseSequentialBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<SequentialBroadcasterStrategy>();
    /// <summary>Configures parallel broadcasting via concurrent task execution.</summary>
    public static WebhookBroadcasterOptions UseParallelTaskBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<ParallelTaskBroadcasterStrategy>();
    /// <summary>Configures broadcasting via the background task processor.</summary>
    public static WebhookBroadcasterOptions UseBackgroundProcessorBroadcasterStrategy(this WebhookBroadcasterOptions options) => options.UseStrategy<BackgroundProcessorBroadcasterStrategy>();
}