using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Webhooks.Core.Options;

/// <summary>
/// An options class that can be used to bind webhook sources from configuration.
/// </summary>
public class WebhookSourcesOptions
{
    public ICollection<WebhookSource> Sources { get; set; } = new List<WebhookSource>();
}

[UsedImplicitly]
public class ConfigureWebhookSourcesOptions(Action<WebhookSourcesOptions>? action) : ConfigureOptions<WebhookSourcesOptions>(action);