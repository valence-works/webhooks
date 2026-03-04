namespace Webhooks.Core;

public interface ITransientFailureDetectionStrategy
{
    bool IsTransient(HttpResponseMessage? response, Exception? exception);
}
