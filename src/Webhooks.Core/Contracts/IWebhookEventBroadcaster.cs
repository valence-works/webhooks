namespace Webhooks.Core;

/// <summary>
/// Broadcasts webhook events to all registered webhook endpoints.
/// </summary>
public interface IWebhookEventBroadcaster
{
    /// <summary>
    /// Broadcasts a webhook event to all registered webhook endpoints.
    /// </summary>
    /// <param name="webhookEvent">The webhook event to broadcast.</param>
    /// <param name="cancellationToken"></param>
    Task BroadcastAsync(NewWebhookEvent webhookEvent, CancellationToken cancellationToken = default);
}