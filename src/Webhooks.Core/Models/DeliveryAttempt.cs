namespace Webhooks.Core;

/// <summary>
/// Records a single delivery attempt to a webhook sink.
/// </summary>
public sealed record DeliveryAttempt(
    string SinkId,
    string EventId,
    string SelectedDispatcher,
    IReadOnlyDictionary<string, string>? AttemptMetadata = null);
