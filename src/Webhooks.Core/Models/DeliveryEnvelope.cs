using System.Text.Json;

namespace Webhooks.Core;

public sealed record DeliveryEnvelope(
    string EventId,
    string EventType,
    JsonElement? Payload,
    DateTimeOffset DispatchTimestamp);
