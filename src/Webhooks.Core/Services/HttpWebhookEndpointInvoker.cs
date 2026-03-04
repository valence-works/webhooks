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

        var maxAttempts = Math.Max(1, options.Value.RetryAttempts);
        var webhookEvent = new WebhookEvent(deliveryEnvelope.EventType, deliveryEnvelope.Payload, deliveryEnvelope.DispatchTimestamp);
        var pipeline = BuildPipeline(SendAsync);

        for (var attempt = 1; attempt <= maxAttempts; attempt++)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, webhookSink.Url)
            {
                Content = JsonContent.Create(webhookEvent)
            };
            request.Headers.TryAddWithoutValidation("X-Webhook-EventId", deliveryEnvelope.EventId);
            request.Headers.TryAddWithoutValidation("X-Webhook-DispatchTimestamp", deliveryEnvelope.DispatchTimestamp.ToString("O"));

            var context = new WebhookEndpointInvocationContext(webhookSink, deliveryEnvelope, attempt, request);
            var result = await pipeline(context, cancellationToken);

            if (result.Status == DeliveryStatus.Succeeded)
            {
                return;
            }

            var responseException = result.FinalFailureReason is null
                ? null
                : new HttpRequestException(result.FinalFailureReason);
            using var probeResponse = result.ResponseStatusCode.HasValue
                ? new HttpResponseMessage(result.ResponseStatusCode.Value)
                : null;
            var isTransient = transientFailureDetectionStrategy.IsTransient(probeResponse, responseException);
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
        try
        {
            using var response = await httpClient.SendAsync(context.Request!, cancellationToken);
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
                "EndpointInvoker",
                response.StatusCode);
        }
        catch (HttpRequestException ex)
        {
            return new DeliveryResult(
                DeliveryStatus.Failed,
                context.Attempt,
                ex.Message,
                context.DeliveryEnvelope.EventId,
                "EndpointInvoker");
        }
        catch (TaskCanceledException ex) when (!cancellationToken.IsCancellationRequested)
        {
            return new DeliveryResult(
                DeliveryStatus.Failed,
                context.Attempt,
                $"Request timed out: {ex.Message}",
                context.DeliveryEnvelope.EventId,
                "EndpointInvoker");
        }
    }
}