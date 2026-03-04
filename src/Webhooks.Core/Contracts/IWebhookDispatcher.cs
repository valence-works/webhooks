namespace Webhooks.Core;

public interface IWebhookDispatcher
{
    /// Transport identifier resolved by coordinator-owned selection policy.
    string Name { get; }

    /// Executes only transport handoff/invocation for a preselected sink attempt.
    /// Selection policy stays coordinator-owned.
    Task<DispatchHandoffResult> DispatchAsync(
        DeliveryEnvelope deliveryEnvelope,
        WebhookSink webhookSink,
        CancellationToken cancellationToken = default);
}
