namespace Webhooks.Core;

public interface IWebhookEndpointInvokerMiddleware
{
    Task<DeliveryResult> InvokeAsync(
        WebhookEndpointInvocationContext context,
        Func<WebhookEndpointInvocationContext, CancellationToken, Task<DeliveryResult>> next,
        CancellationToken cancellationToken = default);
}

public sealed record WebhookEndpointInvocationContext(
    WebhookSink WebhookSink,
    DeliveryEnvelope DeliveryEnvelope,
    int Attempt);
