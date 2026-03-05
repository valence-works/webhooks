namespace Webhooks.Core;

/// <summary>
/// Indicates the outcome of handing off an event to a dispatcher.
/// </summary>
public enum DispatchHandoffStatus
{
    Accepted,
    Enqueued,
    Rejected
}
