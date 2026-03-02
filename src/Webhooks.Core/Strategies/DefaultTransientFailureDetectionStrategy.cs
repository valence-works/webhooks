using System.Net;
using System.Net.Http;

namespace Webhooks.Core.Strategies;

public class DefaultTransientFailureDetectionStrategy : ITransientFailureDetectionStrategy
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
