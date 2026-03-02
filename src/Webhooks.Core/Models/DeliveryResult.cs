using System.Net;

namespace Webhooks.Core;

public enum DeliveryStatus
{
    Pending,
    Succeeded,
    Failed
}

public sealed record DeliveryResult(
    DeliveryStatus Status,
    int AttemptCount,
    string? FinalFailureReason,
    string EventIdCorrelation,
    string OutcomeSource = "EndpointInvoker",
    HttpStatusCode? ResponseStatusCode = null);
