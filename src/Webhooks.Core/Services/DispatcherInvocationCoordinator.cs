using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;

namespace Webhooks.Core.Services;

/// <summary>
/// Selects and invokes the appropriate dispatcher for a webhook delivery.
/// </summary>
public sealed class DispatcherInvocationCoordinator(
    IEnumerable<IWebhookDispatcher> dispatchers,
    IOptions<WebhookBroadcasterOptions> options,
    ILogger<DispatcherInvocationCoordinator> logger) : IDispatcherInvocationCoordinator
{
    public async Task<DispatchHandoffResult> DispatchAsync(
        DeliveryEnvelope deliveryEnvelope,
        WebhookSink webhookSink,
        CancellationToken cancellationToken = default)
    {
        var selectedDispatcherName = webhookSink.Dispatcher ?? options.Value.DefaultDispatcher;

        var selectedDispatcher = SelectDispatcher(dispatchers, selectedDispatcherName);
        if (selectedDispatcher is null)
        {
            logger.LogWarning(
                "No dispatcher available for sink {SinkId}. Requested dispatcher: {RequestedDispatcher}",
                webhookSink.SinkId,
                selectedDispatcherName ?? "<none>");

            return new DispatchHandoffResult(
                selectedDispatcherName ?? "<none>",
                DispatchHandoffStatus.Rejected,
                deliveryEnvelope.EventId,
                "Dispatcher unavailable");
        }

        var handoffResult = await selectedDispatcher.DispatchAsync(deliveryEnvelope, webhookSink, cancellationToken);
        var adjustedResult = ApplyOverflowPolicyIfNeeded(handoffResult);

        logger.LogInformation(
            "Dispatcher handoff for sink {SinkId} completed with status {Status} via {DispatcherName} (eventId: {EventId})",
            webhookSink.SinkId,
            adjustedResult.HandoffStatus,
            adjustedResult.DispatcherName,
            adjustedResult.EventIdCorrelation);

        if (adjustedResult.HandoffStatus == DispatchHandoffStatus.Enqueued)
        {
            logger.LogInformation(
                "Delivery for sink {SinkId} is pending invoke-plane completion (eventId: {EventId})",
                webhookSink.SinkId,
                adjustedResult.EventIdCorrelation);
        }

        return adjustedResult;
    }

    private DispatchHandoffResult ApplyOverflowPolicyIfNeeded(DispatchHandoffResult handoffResult)
    {
        if (handoffResult.HandoffStatus != DispatchHandoffStatus.Rejected)
        {
            return handoffResult;
        }

        if (!handoffResult.IsOverflow)
        {
            return handoffResult;
        }

        return options.Value.OverflowPolicy switch
        {
            OverflowPolicy.FailFast => handoffResult,
            OverflowPolicy.DropOldest => handoffResult with
            {
                HandoffStatus = DispatchHandoffStatus.Enqueued,
                HandoffReason = "Overflow overridden by DropOldest policy"
            },
            OverflowPolicy.Block => handoffResult with
            {
                HandoffStatus = DispatchHandoffStatus.Enqueued,
                HandoffReason = "Overflow overridden by Block policy"
            },
            _ => handoffResult
        };
    }

    private static IWebhookDispatcher? SelectDispatcher(
        IEnumerable<IWebhookDispatcher> availableDispatchers,
        string? selectedDispatcherName)
    {
        var materialized = availableDispatchers.ToList();
        if (materialized.Count == 0)
        {
            return null;
        }

        if (string.IsNullOrWhiteSpace(selectedDispatcherName))
        {
            return materialized[0];
        }

        return materialized.FirstOrDefault(
            x => string.Equals(x.Name, selectedDispatcherName, StringComparison.OrdinalIgnoreCase));
    }
}
