namespace Webhooks.Core;

/// <summary>
/// Invokes a webhook endpoint with a given event payload.
/// </summary>
public interface IWebhookEndpointInvoker
{
    /// <summary>
    /// Sends a webhook event to the specified sink endpoint.
    /// </summary>
    /// <param name="webhookSink">The target webhook sink.</param>
    /// <param name="newWebhookEvent">The event to deliver.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task InvokeAsync(WebhookSink webhookSink, NewWebhookEvent newWebhookEvent, CancellationToken cancellationToken = default);
}