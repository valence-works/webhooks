namespace Webhooks.Core;

/// <summary>
/// Extension methods for broadcasting webhook events with simplified signatures.
/// </summary>
public static class WebhookEventBroadcasterExtensions
{
    /// <summary>
    /// Broadcasts a webhook event to all registered webhook endpoints.
    /// </summary>
    /// <param name="webhookEventBroadcaster">The extended service.</param>
    /// <param name="eventType">The webhook event to broadcast.</param>
    /// <param name="cancellationToken"></param>
    public static Task BroadcastAsync(this IWebhookEventBroadcaster webhookEventBroadcaster, string eventType, CancellationToken cancellationToken = default)
    {
        var webhook = new NewWebhookEvent(eventType);
        return webhookEventBroadcaster.BroadcastAsync(webhook, cancellationToken);
    }
    
    /// <summary>
    /// Broadcasts a webhook event to all registered webhook endpoints.
    /// </summary>
    /// <param name="webhookEventBroadcaster">The extended service.</param>
    /// <param name="eventType">The webhook event to broadcast.</param>
    /// <param name="payload">The webhook event payload to include.</param>
    /// <param name="cancellationToken"></param>
    public static Task BroadcastAsync(this IWebhookEventBroadcaster webhookEventBroadcaster, string eventType, object? payload, CancellationToken cancellationToken = default)
    {
        var webhook = new NewWebhookEvent(eventType, payload);
        return webhookEventBroadcaster.BroadcastAsync(webhook, cancellationToken);
    }
}