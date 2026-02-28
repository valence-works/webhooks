using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Webhooks.Core.Options;

/// <summary>
/// An options class that can be used to bind webhook sinks from configuration.
/// </summary>
public class WebhookSinksOptions
{
    public ICollection<WebhookSink> Sinks { get; set; } = new List<WebhookSink>();
}

[UsedImplicitly]
public class ConfigureWebhookSinksOptions(Action<WebhookSinksOptions>? action) : ConfigureOptions<WebhookSinksOptions>(action);