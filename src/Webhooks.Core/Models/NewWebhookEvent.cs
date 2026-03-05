namespace Webhooks.Core;

/// <summary>
/// Represents a new webhook event to be broadcast to registered sinks.
/// </summary>
public sealed record NewWebhookEvent(
	string EventType,
	object? Payload = null,
	string? EventId = null,
	DateTimeOffset? DispatchTimestamp = null);