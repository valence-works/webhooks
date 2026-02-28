using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Webhooks.Core.Services;

/// <summary>
/// A webhook event broadcaster that sends HTTP requests one by one.
/// </summary>
public class DefaultWebhookEventBroadcaster(
    IWebhookSinkProvider webhookSinkProvider,
    IWebhookEndpointInvoker webhookEndpointInvoker,
    IBroadcasterStrategy strategy,
    ILogger<DefaultWebhookEventBroadcaster> logger) : IWebhookEventBroadcaster
{
    public async Task BroadcastAsync(NewWebhookEvent webhookEvent, CancellationToken cancellationToken = default)
    {
        var webhookSinks = (await webhookSinkProvider.ListAsync(cancellationToken)).ToList();
        var serializedPayload = webhookEvent.Payload != null ? JsonSerializer.SerializeToElement(webhookEvent.Payload) : default;
        
        var query = from webhookSink in webhookSinks.AsQueryable()
            from eventFilter in webhookSink.Filters
            where eventFilter.EventType == webhookEvent.EventType
            where eventFilter.PayloadFilters.Count == 0 || eventFilter.PayloadFilters.Any(x => serializedPayload.GetProperty(x.Key).GetString() == x.Value)
            select webhookSink;

        var matchingWebhookSinks = query.ToList();
        await strategy.BroadcastAsync(matchingWebhookSinks, InvokeEndpointAsync, cancellationToken);
        return;

        async Task InvokeEndpointAsync(WebhookSink endpoint) => await SendWebhookEventAsync(webhookEvent, endpoint, cancellationToken);
    }

    private async Task SendWebhookEventAsync(NewWebhookEvent webhookEvent, WebhookSink sink, CancellationToken cancellationToken)
    {
        try
        {
            await webhookEndpointInvoker.InvokeAsync(sink, webhookEvent, cancellationToken);
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error while sending webhook event {EventType} to {Url}.", webhookEvent.EventType, sink.Url);
        }
    }
}