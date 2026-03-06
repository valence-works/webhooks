using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Routing;

public sealed class LiteralEventTypeCaseSensitivityTests
{
    [Fact]
    public async Task BroadcastAsync_LiteralSubscription_IsCaseSensitive()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "Order.Created" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.Empty(coordinator.DispatchedSinkIds);
    }

    [Fact]
    public async Task BroadcastAsync_LiteralSubscription_ExactCaseMatches()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "Heartbeat" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("Heartbeat", new { id = "1" }));

        Assert.Single(coordinator.DispatchedSinkIds);
        Assert.Equal("sink-a", coordinator.DispatchedSinkIds.Single());
    }

    [Fact]
    public async Task BroadcastAsync_LiteralSubscription_UpperCaseVariantDoesNotMatch()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "Heartbeat" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("HEARTBEAT", new { id = "1" }));

        Assert.Empty(coordinator.DispatchedSinkIds);
    }

    [Fact]
    public async Task BroadcastAsync_LiteralSubscription_LowerCaseVariantDoesNotMatch()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "Heartbeat" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("heartbeat", new { id = "1" }));

        Assert.Empty(coordinator.DispatchedSinkIds);
    }

    private static DefaultWebhookEventBroadcaster CreateBroadcaster(IWebhookSinkProvider sinkProvider, RecordingCoordinator coordinator)
    {
        return new DefaultWebhookEventBroadcaster(
            sinkProvider,
            coordinator,
            new StaticClock(new DateTimeOffset(2026, 03, 01, 12, 0, 0, TimeSpan.Zero)),
            new SequentialBroadcasterStrategy(),
            new WildcardEventTypeMatcherStrategy(NullLogger<WildcardEventTypeMatcherStrategy>.Instance),
            Array.Empty<IBroadcastMiddleware>(),
            new[] { new JsonPathPayloadFieldSelectorStrategy() },
            new[] { new ScalarStringEqualityComparisonStrategy() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions()),
            NullLogger<DefaultWebhookEventBroadcaster>.Instance);
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
