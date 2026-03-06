namespace Webhooks.Core.Strategies;

/// <summary>
/// Event-type matching strategy that only supports case-sensitive exact matching.
/// Hosts can register this strategy to disable wildcard behavior.
/// </summary>
public sealed class ExactEventTypeMatcherStrategy : IEventTypeMatcherStrategy
{
    /// <inheritdoc />
    public bool IsMatch(string? subscriptionEventType, string? incomingEventType)
    {
        return string.Equals(subscriptionEventType, incomingEventType, StringComparison.Ordinal);
    }
}
