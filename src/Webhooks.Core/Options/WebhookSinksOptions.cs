namespace Webhooks.Core.Options;

/// <summary>
/// An options class that can be used to bind webhook sinks from configuration.
/// </summary>
public sealed class WebhookSinksOptions
{
    /// <summary>
    /// Gets or sets the collection of webhook sinks.
    /// </summary>
    public ICollection<WebhookSink> Sinks { get; set; } = new List<WebhookSink>();
}