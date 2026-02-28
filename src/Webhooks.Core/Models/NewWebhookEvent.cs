namespace Webhooks.Core;

public record NewWebhookEvent(string EventType, object? Payload = null);