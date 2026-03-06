using Microsoft.Extensions.Logging.Abstractions;
using Webhooks.Core.Options;
using Webhooks.Core.Services;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Routing;

public sealed class CustomEventTypeMatcherOverrideTests
{
    [Fact]
    public async Task BroadcastAsync_WithCustomMatcher_UsesCustomMatchingLogic()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-a",
                Destination = new Uri("https://example.com/a"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "ORDER.CREATED" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        // Use a custom case-insensitive matcher
        var broadcaster = new DefaultWebhookEventBroadcaster(
            sinkProvider,
            coordinator,
            new StaticClock(new DateTimeOffset(2026, 03, 01, 12, 0, 0, TimeSpan.Zero)),
            new SequentialBroadcasterStrategy(),
            new CaseInsensitiveEventTypeMatcher(),
            Array.Empty<IBroadcastMiddleware>(),
            new[] { new JsonPathPayloadFieldSelectorStrategy() },
            new[] { new ScalarStringEqualityComparisonStrategy() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions()),
            NullLogger<DefaultWebhookEventBroadcaster>.Instance);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.Single(coordinator.DispatchedSinkIds);
        Assert.Equal("sink-a", coordinator.DispatchedSinkIds.Single());
    }

    [Fact]
    public async Task BroadcastAsync_WithExactMatcher_DisablesWildcard()
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
        // Use exact matcher which treats "*" as a literal string
        var broadcaster = new DefaultWebhookEventBroadcaster(
            sinkProvider,
            coordinator,
            new StaticClock(new DateTimeOffset(2026, 03, 01, 12, 0, 0, TimeSpan.Zero)),
            new SequentialBroadcasterStrategy(),
            new ExactEventTypeMatcherStrategy(),
            Array.Empty<IBroadcastMiddleware>(),
            new[] { new JsonPathPayloadFieldSelectorStrategy() },
            new[] { new ScalarStringEqualityComparisonStrategy() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions()),
            NullLogger<DefaultWebhookEventBroadcaster>.Instance);

        await broadcaster.BroadcastAsync(new NewWebhookEvent("order.created", new { id = "1" }));

        Assert.Empty(coordinator.DispatchedSinkIds);
    }

    [Fact]
    public async Task BroadcastAsync_WithExactMatcher_LiteralStarMatchesStar()
    {
        var sinkProvider = new TestWebhookSinkProvider(new[]
        {
            new WebhookSink
            {
                SinkId = "sink-star",
                Destination = new Uri("https://example.com/star"),
                Subscriptions = new List<WebhookEventFilter>
                {
                    new() { EventType = "*" }
                }
            }
        });

        var coordinator = new RecordingCoordinator();
        var broadcaster = new DefaultWebhookEventBroadcaster(
            sinkProvider,
            coordinator,
            new StaticClock(new DateTimeOffset(2026, 03, 01, 12, 0, 0, TimeSpan.Zero)),
            new SequentialBroadcasterStrategy(),
            new ExactEventTypeMatcherStrategy(),
            Array.Empty<IBroadcastMiddleware>(),
            new[] { new JsonPathPayloadFieldSelectorStrategy() },
            new[] { new ScalarStringEqualityComparisonStrategy() },
            Microsoft.Extensions.Options.Options.Create(new WebhookBroadcasterOptions()),
            NullLogger<DefaultWebhookEventBroadcaster>.Instance);

        // Event type is literally "*"
        await broadcaster.BroadcastAsync(new NewWebhookEvent("*", new { id = "1" }));

        Assert.Single(coordinator.DispatchedSinkIds);
        Assert.Equal("sink-star", coordinator.DispatchedSinkIds.Single());
    }

    /// <summary>
    /// A custom matcher that performs case-insensitive comparison,
    /// demonstrating the host override extensibility point.
    /// </summary>
    private sealed class CaseInsensitiveEventTypeMatcher : IEventTypeMatcherStrategy
    {
        public bool IsMatch(string? subscriptionEventType, string? incomingEventType)
        {
            return string.Equals(subscriptionEventType, incomingEventType, StringComparison.OrdinalIgnoreCase);
        }
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
