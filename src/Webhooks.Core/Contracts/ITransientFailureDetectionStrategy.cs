namespace Webhooks.Core;

/// <summary>
/// Determines whether a delivery failure is transient and eligible for retry.
/// </summary>
public interface ITransientFailureDetectionStrategy
{
    /// <summary>
    /// Evaluates whether the failure indicated by the response or exception is transient.
    /// </summary>
    /// <param name="response">The HTTP response, if available.</param>
    /// <param name="exception">The exception, if one was thrown.</param>
    /// <returns><c>true</c> if the failure is transient; otherwise <c>false</c>.</returns>
    bool IsTransient(HttpResponseMessage? response, Exception? exception);
}
