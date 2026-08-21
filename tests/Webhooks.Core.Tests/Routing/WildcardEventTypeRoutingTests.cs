using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Routing;

public sealed class WildcardEventTypeRoutingTests
{
    [Fact]
    public async Task BroadcastAsync_WildcardSubscription_MatchesAnyEventType()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-wildcard",
                Destination = new Uri("https://example.com/wildcard"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "*" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.Single(coordinator.DispatchedSinkIds);
        Assert.Equal("sink-wildcard", coordinator.DispatchedSinkIds.Single());
    }

    [Fact]
    public async Task BroadcastAsync_WildcardSubscription_MatchesMultipleDifferentEventTypes()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-wildcard",
                Destination = new Uri("https://example.com/wildcard"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "*" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));
        await broadcaster.BroadcastAsync(new NewWebhookEvent("invoice.paid", new { id = "2" }));
        await broadcaster.BroadcastAsync(new NewWebhookEvent("customer.deleted", new { id = "3" }));

        Assert.Equal(3, coordinator.DispatchedSinkIds.Count);
        Assert.All(coordinator.DispatchedSinkIds, id => Assert.Equal("sink-wildcard", id));
    }

    [Fact]
    public async Task BroadcastAsync_WildcardAndLiteralSubscriptions_BothMatchCorrectly()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-wildcard",
                Destination = new Uri("https://example.com/wildcard"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "*" }
                }
            },
            new WebhookSink
            {
                SinkId = "sink-literal",
                Destination = new Uri("https://example.com/literal"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "order.created" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.Equal(2, coordinator.DispatchedSinkIds.Count);
        Assert.Contains("sink-wildcard", coordinator.DispatchedSinkIds);
        Assert.Contains("sink-literal", coordinator.DispatchedSinkIds);
    }

    [Fact]
    public async Task BroadcastAsync_WildcardSubscription_MatchesEventTypeWithDots()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-wildcard",
                Destination = new Uri("https://example.com/wildcard"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "*" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("org.division.team.order.created", new { id = "1" }));

        Assert.Single(coordinator.DispatchedSinkIds);
        Assert.Equal("sink-wildcard", coordinator.DispatchedSinkIds.Single());
    }

    [Fact]
    public async Task BroadcastAsync_OnlyWildcardSink_NoLiteralMatch_StillDelivers()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-wildcard",
                Destination = new Uri("https://example.com/wildcard"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "*" }
                }
            },
            new WebhookSink
            {
                SinkId = "sink-other",
                Destination = new Uri("https://example.com/other"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "invoice.paid" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = CreateBroadcaster(sinkProvider, coordinator);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.Single(coordinator.DispatchedSinkIds);
        Assert.Equal("sink-wildcard", coordinator.DispatchedSinkIds.Single());
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
