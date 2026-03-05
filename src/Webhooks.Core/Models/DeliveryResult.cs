using System.Net;

namespace Webhooks.Core;

/// <summary>
/// Contains the result of delivering a webhook event to a single sink.
/// </summary>
public sealed record DeliveryResult(
    DeliveryStatus Status,
    int AttemptCount,
    string? FinalFailureReason,
    string EventIdCorrelation,
    string OutcomeSource = "EndpointInvoker",
    HttpStatusCode? ResponseStatusCode = null);
