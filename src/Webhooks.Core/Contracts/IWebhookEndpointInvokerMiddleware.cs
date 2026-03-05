namespace Webhooks.Core;

/// <summary>
/// Middleware that participates in the webhook endpoint invocation pipeline.
/// </summary>
public interface IWebhookEndpointInvokerMiddleware
{
    /// <summary>
    /// Processes a webhook invocation, optionally delegating to the next middleware.
    /// </summary>
    /// <param name="context">The invocation context containing sink and envelope details.</param>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    Task<DeliveryResult> InvokeAsync(
        WebhookEndpointInvocationContext context,
        Func<WebhookEndpointInvocationContext, CancellationToken, Task<DeliveryResult>> next,
        CancellationToken cancellationToken = default);
}
