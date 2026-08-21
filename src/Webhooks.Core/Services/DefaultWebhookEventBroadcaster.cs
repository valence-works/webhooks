using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Services;

/// <summary>
/// A webhook event broadcaster that sends HTTP requests one by one.
/// </summary>
public sealed class DefaultWebhookEventBroadcaster : IWebhookEventBroadcaster
{
    private readonly IWebhookSinkProvider webhookSinkProvider;
    private readonly IDispatcherInvocationCoordinator dispatcherInvocationCoordinator;
    private readonly ISystemClock systemClock;
    private readonly IBroadcasterStrategy strategy;
    private readonly IEventTypeMatcherStrategy eventTypeMatcher;
    private readonly ILogger<DefaultWebhookEventBroadcaster> logger;
    private readonly IPayloadFieldSelectorStrategy selector;
    private readonly IPayloadValueComparisonStrategy comparator;
    private readonly IReadOnlyList<IBroadcastMiddleware> broadcastMiddlewares;
    private readonly IOptions<WebhookBroadcasterOptions> broadcasterOptions;
    private readonly HashSet<string> seenEventIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Queue<string> seenEventIdOrder = new();
    private readonly object eventIdGate = new();

    public DefaultWebhookEventBroadcaster(
        IWebhookSinkProvider webhookSinkProvider,
        IDispatcherInvocationCoordinator dispatcherInvocationCoordinator,
        ISystemClock systemClock,
        IBroadcasterStrategy strategy,
        IEventTypeMatcherStrategy eventTypeMatcher,
        IEnumerable<IBroadcastMiddleware> broadcastMiddlewares,
        IEnumerable<IPayloadFieldSelectorStrategy> payloadFieldSelectorStrategies,
        IEnumerable<IPayloadValueComparisonStrategy> payloadValueComparisonStrategies,
        IOptions<WebhookBroadcasterOptions> broadcasterOptions,
        ILogger<DefaultWebhookEventBroadcaster> logger)
    {
        this.webhookSinkProvider = webhookSinkProvider;
        this.dispatcherInvocationCoordinator = dispatcherInvocationCoordinator;
        this.systemClock = systemClock;
        this.strategy = strategy;
        this.eventTypeMatcher = eventTypeMatcher;
        this.broadcasterOptions = broadcasterOptions;
        this.logger = logger;
        this.broadcastMiddlewares = broadcastMiddlewares.ToList();
        this.selector = payloadFieldSelectorStrategies.LastOrDefault() ?? new JsonPathPayloadFieldSelectorStrategy();
        this.comparator = payloadValueComparisonStrategies.LastOrDefault() ?? new ScalarStringEqualityComparisonStrategy();
    }

    public async Task BroadcastAsync(NewWebhookEvent webhookEvent, CancellationToken cancellationToken = default)
    {
        var webhookSinks = (await webhookSinkProvider.ListAsync(cancellationToken)).ToList();
        var normalizedEventId = string.IsNullOrWhiteSpace(webhookEvent.EventId)
            ? Guid.NewGuid().ToString("N")
            : webhookEvent.EventId;
        var dispatchTimestamp = webhookEvent.DispatchTimestamp ?? systemClock.UtcNow;

        if (ShouldSkipDuplicate(normalizedEventId!))
        {
            return;
        }

        var serializedPayload = webhookEvent.Payload != null ? JsonSerializer.SerializeToElement(webhookEvent.Payload) : default(JsonElement?);
        var deliveryEnvelope = new DeliveryEnvelope(
            normalizedEventId!,
            webhookEvent.EventType,
            serializedPayload,
            dispatchTimestamp);
        
        Task DispatchCore(DeliveryEnvelope envelope, CancellationToken token)
        {
            var query = from webhookSink in webhookSinks.AsQueryable()
                from eventFilter in webhookSink.Subscriptions
                where eventTypeMatcher.IsMatch(eventFilter.EventType, webhookEvent.EventType)
                where eventFilter.PayloadFilters.Count == 0 || PayloadMatches(eventFilter, serializedPayload)
                select webhookSink;

            var matchingWebhookSinks = query.ToList();
            return strategy.BroadcastAsync(matchingWebhookSinks, InvokeEndpointAsync, token);

            async Task InvokeEndpointAsync(WebhookSink endpoint) => await SendWebhookEventAsync(envelope, endpoint, token);
        }

        var pipeline = BuildBroadcastPipeline(DispatchCore);
        await pipeline(deliveryEnvelope, cancellationToken);
        return;
    }

    private async Task SendWebhookEventAsync(DeliveryEnvelope deliveryEnvelope, WebhookSink sink, CancellationToken cancellationToken)
    {
        try
        {
            await dispatcherInvocationCoordinator.DispatchAsync(deliveryEnvelope, sink, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while dispatching webhook event {EventType} to {Url}.", deliveryEnvelope.EventType, sink.Url);
        }
    }

    private bool PayloadMatches(SubscriptionCriteria eventFilter, JsonElement? serializedPayload)
    {
        if (eventFilter.PayloadFilters.Count == 0)
        {
            return true;
        }

        if (serializedPayload is null)
        {
            return false;
        }

        var predicateResults = eventFilter.PayloadFilters
            .Select(payloadFilter =>
            {
                var matched = selector.TrySelect(serializedPayload.Value, payloadFilter.Selector, out var selectedValue)
                              && comparator.IsMatch(selectedValue, payloadFilter.ExpectedValue);
                return matched;
            })
            .ToList();

        var mode = eventFilter.PayloadMatchingMode ?? PayloadMatchingMode.Or;
        return mode == PayloadMatchingMode.And
            ? predicateResults.All(x => x)
            : predicateResults.Any(x => x);
    }

    private bool ShouldSkipDuplicate(string eventId)
    {
        if (!broadcasterOptions.Value.DeduplicationEnabled)
        {
            return false;
        }

        lock (eventIdGate)
        {
            if (seenEventIds.Contains(eventId))
            {
                return true;
            }

            var maxEntries = broadcasterOptions.Value.MaxDeduplicationEntries;
            if (maxEntries > 0 && seenEventIds.Count >= maxEntries)
            {
                var oldest = seenEventIdOrder.Dequeue();
                seenEventIds.Remove(oldest);
            }

            seenEventIds.Add(eventId);
            seenEventIdOrder.Enqueue(eventId);
            return false;
        }
    }

    private Func<DeliveryEnvelope, CancellationToken, Task> BuildBroadcastPipeline(
        Func<DeliveryEnvelope, CancellationToken, Task> terminal)
    {
        Func<DeliveryEnvelope, CancellationToken, Task> current = terminal;
        foreach (var middleware in broadcastMiddlewares.Reverse())
        {
            var next = current;
            current = (envelope, token) => middleware.InvokeAsync(envelope, next, token);
        }

        return current;
    }
}