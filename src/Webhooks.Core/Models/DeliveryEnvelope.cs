using System.Text.Json;

namespace Webhooks.Core;

/// <summary>
/// An immutable envelope containing the payload and metadata for a webhook delivery.
/// </summary>
public sealed record DeliveryEnvelope(
    string EventId,
    string EventType,
    JsonElement? Payload,
    DateTimeOffset DispatchTimestamp);
