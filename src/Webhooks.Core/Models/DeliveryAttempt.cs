namespace Webhooks.Core;

public sealed record DeliveryAttempt(
    string SinkId,
    string EventId,
    string SelectedDispatcher,
    IReadOnlyDictionary<string, string>? AttemptMetadata = null);
