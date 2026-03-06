using Microsoft.Extensions.Logging;

namespace Webhooks.Core.Strategies;

/// <summary>
/// Default event-type matching strategy. Supports wildcard (<c>*</c>) subscriptions that match
/// any incoming event type, and case-sensitive exact matching for literal subscription values.
/// Subscription values with leading/trailing whitespace are treated as invalid configuration,
/// logged as a warning, and evaluated as non-match.
/// </summary>
public sealed class WildcardEventTypeMatcherStrategy : IEventTypeMatcherStrategy
{
    private const string WildcardToken = "*";
    private readonly ILogger<WildcardEventTypeMatcherStrategy> logger;

    public WildcardEventTypeMatcherStrategy(ILogger<WildcardEventTypeMatcherStrategy> logger)
    {
        this.logger = logger;
    }

    /// <inheritdoc />
    public bool IsMatch(string? subscriptionEventType, string? incomingEventType)
    {
        // Null/empty/whitespace-only subscription is invalid → no match.
        if (string.IsNullOrWhiteSpace(subscriptionEventType))
        {
            logger.LogWarning("Subscription event type is null, empty, or whitespace-only and will not match any incoming event type.");
            return false;
        }

        // Leading/trailing whitespace is invalid configuration → warn and no match.
        if (subscriptionEventType != subscriptionEventType.Trim())
        {
            logger.LogWarning("Subscription event type '{SubscriptionEventType}' has leading or trailing whitespace and is treated as invalid configuration.", subscriptionEventType);
            return false;
        }

        // Wildcard matches any incoming event type (including null/empty/whitespace).
        if (subscriptionEventType == WildcardToken)
        {
            return true;
        }

        // Case-sensitive exact match for literal subscription values.
        return string.Equals(subscriptionEventType, incomingEventType, StringComparison.Ordinal);
    }
}
