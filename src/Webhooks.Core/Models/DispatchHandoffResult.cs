namespace Webhooks.Core;

public enum DispatchHandoffStatus
{
    Accepted,
    Enqueued,
    Rejected
}

public sealed record DispatchHandoffResult(
    string DispatcherName,
    DispatchHandoffStatus HandoffStatus,
    string EventIdCorrelation,
    string? HandoffReason = null,
    bool IsOverflow = false);
