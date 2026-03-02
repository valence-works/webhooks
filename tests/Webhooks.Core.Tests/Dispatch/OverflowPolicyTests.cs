using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;

namespace Webhooks.Core.Tests.Dispatch;

public class OverflowPolicyTests
{
    [Fact]
    public async Task DispatchAsync_Converts_Overflow_Rejection_To_Enqueued_For_Block_Policy()
    {
        var coordinator = new DispatcherInvocationCoordinator(
            new[] { new OverflowDispatcher() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions { OverflowPolicy = OverflowPolicy.Block }),
            NullLogger<DispatcherInvocationCoordinator>.Instance);

        var result = await coordinator.DispatchAsync(
            new DeliveryEnvelope("evt-1", "order.created", null, DateTimeOffset.UtcNow),
            new WebhookSink { SinkId = "sink-a", Destination = new Uri("https://example.com/a") });

        Assert.Equal(DispatchHandoffStatus.Enqueued, result.HandoffStatus);
    }

    private sealed class OverflowDispatcher : IWebhookDispatcher
    {
        public string Name => "default";

        public Task<DispatchHandoffResult> DispatchAsync(DeliveryEnvelope deliveryEnvelope, WebhookSink webhookSink, CancellationToken cancellationToken = default)
            => Task.FromResult(new DispatchHandoffResult(Name, DispatchHandoffStatus.Rejected, deliveryEnvelope.EventId, "capacity reached"));
    }
}
