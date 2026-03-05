namespace Webhooks.Core;

/// <summary>
/// Indicates the outcome of a webhook delivery attempt.
/// </summary>
public enum DeliveryStatus
{
    Pending,
    Succeeded,
    Failed
}
