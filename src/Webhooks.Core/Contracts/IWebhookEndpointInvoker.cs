namespace Webhooks.Core;

public interface IWebhookEndpointInvoker
{
    Task InvokeAsync(WebhookSink webhookSink, NewWebhookEvent newWebhookEvent, CancellationToken cancellationToken = default);
}