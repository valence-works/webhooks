using System.Net;

namespace Webhooks.Core.Strategies;

/// <summary>
/// Default strategy that classifies HTTP 5xx, 408, and 429 responses as transient failures.
/// </summary>
public sealed class DefaultTransientFailureDetectionStrategy : ITransientFailureDetectionStrategy
{
    public bool IsTransient(HttpResponseMessage? response, Exception? exception)
    {
        if (exception is HttpRequestException or TaskCanceledException)
        {
            return true;
        }

        if (response is null)
        {
            return false;
        }

        var statusCode = (int)response.StatusCode;
        return statusCode >= (int)HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.RequestTimeout;
    }
}
