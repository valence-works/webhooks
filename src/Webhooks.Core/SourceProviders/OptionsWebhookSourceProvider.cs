using Microsoft.Extensions.Options;
using Webhooks.Core.Options;

namespace Webhooks.Core.SourceProviders;

/// A webhook endpoints source that provides webhook endpoints from configuration via <see ref="WebhookSinksOptions"/>. 
public class OptionsWebhookSourceProvider(IOptionsMonitor<WebhookSourcesOptions> optionsMonitor) : IWebhookSourceProvider
{
    public ValueTask<IEnumerable<WebhookSource>> ListAsync(CancellationToken cancellationToken = default)
    {
        var sources = optionsMonitor.CurrentValue.Sources;
        return new(sources);
    }
}