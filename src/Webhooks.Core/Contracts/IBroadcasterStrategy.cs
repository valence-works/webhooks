namespace Webhooks.Core;

/// <summary>
/// Defines the strategy for broadcasting webhook events to multiple sinks.
/// </summary>
public interface IBroadcasterStrategy
{
    /// <summary>
    /// Broadcasts to each matching webhook endpoint using the provided invocation delegate.
    /// </summary>
    /// <param name="webhookEndpoints">The webhook sinks to broadcast to.</param>
    /// <param name="invocation">The delegate that performs delivery to a single sink.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task BroadcastAsync(IEnumerable<WebhookSink> webhookEndpoints, Func<WebhookSink, Task> invocation, CancellationToken cancellationToken = default);
}