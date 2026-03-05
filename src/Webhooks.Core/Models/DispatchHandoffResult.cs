namespace Webhooks.Core;

/// <summary>
/// Contains the result of dispatching a delivery envelope to a dispatcher.
/// </summary>
public sealed record DispatchHandoffResult(
    string DispatcherName,
    DispatchHandoffStatus HandoffStatus,
    string EventIdCorrelation,
    string? HandoffReason = null,
    bool IsOverflow = false);
