using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Dispatch;

public sealed class DeduplicationPolicyTests
{
    [Fact]
    public async Task BroadcastAsync_Skips_Duplicate_EventId_When_Deduplication_Enabled()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "order.created" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = new DefaultWebhookEventBroadcaster(
            sinkProvider,
            coordinator,
            new StaticClock(new DateTimeOffset(2026, 03, 01, 12, 0, 0, TimeSpan.Zero)),
            new SequentialBroadcasterStrategy(),
            Array.Empty<IBroadcastMiddleware>(),
            new[] { new JsonPathPayloadFieldSelectorStrategy() },
            new[] { new ScalarStringEqualityComparisonStrategy() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions { DeduplicationEnabled = true }),
            NullLogger<DefaultWebhookEventBroadcaster>.Instance);

        var webhookEvent = new NewWebhookEvent("order.created", new { id = "1" }, EventId: "evt-1");

        await broadcaster.BroadcastAsync(webhookEvent);
        await broadcaster.BroadcastAsync(webhookEvent);

        Assert.Single(coordinator.DispatchedSinkIds);
    }

    private sealed class TestWebhookSinkProvider(IEnumerable<WebhookSink> sinks) : IWebhookSinkProvider
    {
        public ValueTask<IEnumerable<WebhookSink>> ListAsync(CancellationToken cancellationToken = default)
            => ValueTask.FromResult(sinks);
    }

    private sealed class RecordingCoordinator : IDispatcherInvocationCoordinator
    {
        public List<string> DispatchedSinkIds { get; } = new();

        public Task<DispatchHandoffResult> DispatchAsync(DeliveryEnvelope deliveryEnvelope, WebhookSink webhookSink, CancellationToken cancellationToken = default)
        {
            DispatchedSinkIds.Add(webhookSink.SinkId);
            return Task.FromResult(new DispatchHandoffResult("default", DispatchHandoffStatus.Accepted, deliveryEnvelope.EventId));
        }
    }

    private sealed class StaticClock(DateTimeOffset value) : ISystemClock
    {
        public DateTimeOffset UtcNow => value;
    }
}
