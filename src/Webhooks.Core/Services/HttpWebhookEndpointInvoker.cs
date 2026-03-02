using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Webhooks.Core.Options;
using WebhooksCore;

namespace Webhooks.Core.Services;

public class HttpWebhookEndpointInvoker(
    HttpClient httpClient,
    ISystemClock systemClock,
    IEnumerable<IWebhookEndpointInvokerMiddleware> middlewares,
    ITransientFailureDetectionStrategy transientFailureDetectionStrategy,
    IOptions<WebhookBroadcasterOptions> options) : IWebhookEndpointInvoker
{
    public async Task InvokeAsync(WebhookSink webhookSink, NewWebhookEvent newWebhookEvent, CancellationToken cancellationToken = default)
    {
        var dispatchTimestamp = newWebhookEvent.DispatchTimestamp ?? systemClock.UtcNow;
        var eventId = string.IsNullOrWhiteSpace(newWebhookEvent.EventId)
            ? Guid.NewGuid().ToString("N")
            : newWebhookEvent.EventId;
        var payload = newWebhookEvent.Payload is null
            ? default(JsonElement?)
            : JsonSerializer.SerializeToElement(newWebhookEvent.Payload);
        var deliveryEnvelope = new DeliveryEnvelope(eventId!, newWebhookEvent.EventType, payload, dispatchTimestamp);

        var maxAttempts = options.Value.RetryAttempts;

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            var context = new WebhookEndpointInvocationContext(webhookSink, deliveryEnvelope, attempt);
            var pipeline = BuildPipeline(SendAsync);
            var result = await pipeline(context, cancellationToken);

            if (result.Status == DeliveryStatus.Succeeded)
            {
                return;
            }

            var responseException = result.FinalFailureReason is null
                ? null
                : new HttpRequestException(result.FinalFailureReason);
            var isTransient = transientFailureDetectionStrategy.IsTransient(null, responseException);
            var canRetry = attempt < maxAttempts;

            if (!isTransient || !canRetry)
            {
                throw responseException ?? new HttpRequestException("Webhook invocation failed.");
            }
        }
    }

    private Func<WebhookEndpointInvocationContext, CancellationToken, Task<DeliveryResult>> BuildPipeline(
        Func<WebhookEndpointInvocationContext, CancellationToken, Task<DeliveryResult>> terminal)
    {
        Func<WebhookEndpointInvocationContext, CancellationToken, Task<DeliveryResult>> current = terminal;
        foreach (var middleware in middlewares.Reverse())
        {
            var next = current;
            current = (context, token) => middleware.InvokeAsync(context, next, token);
        }

        return current;
    }

    private async Task<DeliveryResult> SendAsync(WebhookEndpointInvocationContext context, CancellationToken cancellationToken)
    {
        var webhookEvent = new WebhookEvent(
            context.DeliveryEnvelope.EventType,
            context.DeliveryEnvelope.Payload,
            context.DeliveryEnvelope.DispatchTimestamp);

        using var request = new HttpRequestMessage(HttpMethod.Post, context.WebhookSink.Url)
        {
            Content = JsonContent.Create(webhookEvent)
        };

        request.Headers.TryAddWithoutValidation("X-Webhook-EventId", context.DeliveryEnvelope.EventId);
        request.Headers.TryAddWithoutValidation("X-Webhook-DispatchTimestamp", context.DeliveryEnvelope.DispatchTimestamp.ToString("O"));

        var response = await httpClient.SendAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            return new DeliveryResult(
                DeliveryStatus.Succeeded,
                context.Attempt,
                null,
                context.DeliveryEnvelope.EventId,
                "EndpointInvoker");
        }

        var failureReason = $"HTTP {(int)response.StatusCode} {response.ReasonPhrase}";
        return new DeliveryResult(
            DeliveryStatus.Failed,
            context.Attempt,
            failureReason,
            context.DeliveryEnvelope.EventId,
            "EndpointInvoker");
    }
}