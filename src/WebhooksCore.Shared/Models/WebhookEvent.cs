// ReSharper disable once CheckNamespace
namespace WebhooksCore;

/// <summary>
/// A webhook event to send to a webhook endpoint.
/// </summary>
public sealed record WebhookEvent(string EventType, object? Payload, DateTimeOffset Timestamp);

/// <summary>
/// A webhook event to send to a webhook endpoint.
/// </summary>
public sealed record WebhookEvent<T>(string EventType, T? Payload, DateTimeOffset Timestamp);