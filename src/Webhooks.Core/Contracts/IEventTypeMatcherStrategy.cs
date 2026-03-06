namespace Webhooks.Core;

/// <summary>
/// Determines whether a subscription event type matches an incoming event type during sink routing.
/// </summary>
public interface IEventTypeMatcherStrategy
{
    /// <summary>
    /// Evaluates whether the subscription event type matches the incoming event type.
    /// </summary>
    /// <param name="subscriptionEventType">The configured subscription event type value.</param>
    /// <param name="incomingEventType">The event type value from the broadcast event.</param>
    /// <returns><c>true</c> if the subscription matches the incoming event type; otherwise <c>false</c>.</returns>
    bool IsMatch(string? subscriptionEventType, string? incomingEventType);
}
