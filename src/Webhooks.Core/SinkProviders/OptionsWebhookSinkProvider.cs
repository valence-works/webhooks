using Microsoft.Extensions.Options;
using Webhooks.Core.Options;

namespace Webhooks.Core.SinkProviders;

/// A webhook endpoints source that provides webhook endpoints from configuration via <see ref="WebhookSinksOptions"/>. 
public class OptionsWebhookSinkProvider(IOptionsMonitor<WebhookSinksOptions> optionsMonitor) : IWebhookSinkProvider
{
    public ValueTask<IEnumerable<WebhookSink>> ListAsync(CancellationToken cancellationToken = default)
    {
        var sinks = optionsMonitor.CurrentValue.Sinks;
        return new(sinks);
    }
}