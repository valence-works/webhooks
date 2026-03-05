namespace Webhooks.Core;

/// <summary>
/// Coordinates dispatcher selection and invocation for webhook delivery.
/// </summary>
public interface IDispatcherInvocationCoordinator
{
    /// <summary>
    /// Dispatches a delivery envelope to the appropriate dispatcher for the given sink.
    /// </summary>
    Task<DispatchHandoffResult> DispatchAsync(
        DeliveryEnvelope deliveryEnvelope,
        WebhookSink webhookSink,
        CancellationToken cancellationToken = default);
}
