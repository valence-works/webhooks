namespace Webhooks.Core;

/// <summary>
/// Handles the transport-level delivery of webhook events to a sink endpoint.
/// </summary>
public interface IWebhookDispatcher
{
    /// <summary>
    /// Transport identifier resolved by coordinator-owned selection policy.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Executes only transport handoff/invocation for a preselected sink attempt.
    /// Selection policy stays coordinator-owned.
    /// </summary>
    Task<DispatchHandoffResult> DispatchAsync(
        DeliveryEnvelope deliveryEnvelope,
        WebhookSink webhookSink,
        CancellationToken cancellationToken = default);
}
