namespace Webhooks.Core;

/// <summary>
/// Encapsulates the context for a single webhook endpoint invocation attempt.
/// </summary>
public sealed record WebhookEndpointInvocationContext(
    WebhookSink WebhookSink,
    DeliveryEnvelope DeliveryEnvelope,
    int Attempt,
    HttpRequestMessage? Request = null);
