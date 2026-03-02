using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Services;

/// <summary>
/// A webhook event broadcaster that sends HTTP requests one by one.
/// </summary>
public class DefaultWebhookEventBroadcaster : IWebhookEventBroadcaster
{
    private readonly IWebhookSinkProvider _webhookSinkProvider;
    private readonly IDispatcherInvocationCoordinator _dispatcherInvocationCoordinator;
    private readonly ISystemClock _systemClock;
    private readonly IBroadcasterStrategy _strategy;
    private readonly ILogger<DefaultWebhookEventBroadcaster> _logger;
    private readonly IPayloadFieldSelectorStrategy _selector;
    private readonly IPayloadValueComparisonStrategy _comparator;
    private readonly IReadOnlyList<IBroadcastMiddleware> _broadcastMiddlewares;
    private readonly IOptions<WebhookBroadcasterOptions> _broadcasterOptions;
    private readonly HashSet<string> _seenEventIds = new(StringComparer.OrdinalIgnoreCase);
    private readonly Queue<string> _seenEventIdOrder = new();
    private readonly object _eventIdGate = new();

    public DefaultWebhookEventBroadcaster(
        IWebhookSinkProvider webhookSinkProvider,
        IDispatcherInvocationCoordinator dispatcherInvocationCoordinator,
        ISystemClock systemClock,
        IBroadcasterStrategy strategy,
        IEnumerable<IBroadcastMiddleware> broadcastMiddlewares,
        IEnumerable<IPayloadFieldSelectorStrategy> payloadFieldSelectorStrategies,
        IEnumerable<IPayloadValueComparisonStrategy> payloadValueComparisonStrategies,
        IOptions<WebhookBroadcasterOptions> broadcasterOptions,
        ILogger<DefaultWebhookEventBroadcaster> logger)
    {
        _webhookSinkProvider = webhookSinkProvider;
        _dispatcherInvocationCoordinator = dispatcherInvocationCoordinator;
        _systemClock = systemClock;
        _strategy = strategy;
        _broadcasterOptions = broadcasterOptions;
        _logger = logger;
        _broadcastMiddlewares = broadcastMiddlewares.ToList();
        _selector = payloadFieldSelectorStrategies.LastOrDefault() ?? new JsonPathPayloadFieldSelectorStrategy();
        _comparator = payloadValueComparisonStrategies.LastOrDefault() ?? new ScalarStringEqualityComparisonStrategy();
    }

    public async Task BroadcastAsync(NewWebhookEvent webhookEvent, CancellationToken cancellationToken = default)
    {
        var webhookSinks = (await _webhookSinkProvider.ListAsync(cancellationToken)).ToList();
        var normalizedEventId = string.IsNullOrWhiteSpace(webhookEvent.EventId)
            ? Guid.NewGuid().ToString("N")
            : webhookEvent.EventId;
        var dispatchTimestamp = webhookEvent.DispatchTimestamp ?? _systemClock.UtcNow;

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
                where eventFilter.EventType == webhookEvent.EventType
                where eventFilter.PayloadFilters.Count == 0 || PayloadMatches(eventFilter, serializedPayload)
                select webhookSink;

            var matchingWebhookSinks = query.ToList();
            return _strategy.BroadcastAsync(matchingWebhookSinks, InvokeEndpointAsync, token);

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
            await _dispatcherInvocationCoordinator.DispatchAsync(deliveryEnvelope, sink, cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while dispatching webhook event {EventType} to {Url}.", deliveryEnvelope.EventType, sink.Url);
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
                var matched = _selector.TrySelect(serializedPayload.Value, payloadFilter.Selector, out var selectedValue)
                              && _comparator.IsMatch(selectedValue, payloadFilter.ExpectedValue);
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
        if (!_broadcasterOptions.Value.DeduplicationEnabled)
        {
            return false;
        }

        lock (_eventIdGate)
        {
            if (_seenEventIds.Contains(eventId))
            {
                return true;
            }

            var maxEntries = _broadcasterOptions.Value.MaxDeduplicationEntries;
            if (maxEntries > 0 && _seenEventIds.Count >= maxEntries)
            {
                var oldest = _seenEventIdOrder.Dequeue();
                _seenEventIds.Remove(oldest);
            }

            _seenEventIds.Add(eventId);
            _seenEventIdOrder.Enqueue(eventId);
            return false;
        }
    }

    private Func<DeliveryEnvelope, CancellationToken, Task> BuildBroadcastPipeline(
        Func<DeliveryEnvelope, CancellationToken, Task> terminal)
    {
        Func<DeliveryEnvelope, CancellationToken, Task> current = terminal;
        foreach (var middleware in _broadcastMiddlewares.Reverse())
        {
            var next = current;
            current = (envelope, token) => middleware.InvokeAsync(envelope, next, token);
        }

        return current;
    }
}