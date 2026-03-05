using Microsoft.Extensions.Options;
using Webhooks.Core.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core;

/// <summary>
/// Validates that webhook sink configurations are consistent.
/// </summary>
public sealed class ValidateWebhookSinksOptions : IValidateOptions<WebhookSinksOptions>
{
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
                    if (!JsonPathPayloadFieldSelectorStrategy.SelectorRegex.IsMatch(payloadFilter.Selector))
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
