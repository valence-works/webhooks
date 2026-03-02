namespace Webhooks.Core;

public interface IDispatcherInvocationCoordinator
{
    Task<DispatchHandoffResult> DispatchAsync(
        DeliveryEnvelope deliveryEnvelope,
        WebhookSink webhookSink,
        CancellationToken cancellationToken = default);
}
