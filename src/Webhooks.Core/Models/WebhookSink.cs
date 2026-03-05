using JetBrains.Annotations;

namespace Webhooks.Core;

/// <summary>
/// Represents a webhook events sink interested in a given set of events.
/// </summary>
[UsedImplicitly]
public sealed class WebhookSink
{
    /// <summary>
    /// A unique identifier for this sink.
    /// </summary>
    public string Id { get; set; } = default!;

    /// <summary>
    /// Alias for <see cref="Id"/>. Provided for configuration compatibility.
    /// </summary>
    public string SinkId
    {
        get => Id;
        set => Id = value;
    }

    /// <summary>
    /// A friendly name for this endpoint.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// The URL to send the webhook event to.
    /// </summary>
    public Uri Url { get; set; } = default!;

    /// <summary>
    /// Alias for <see cref="Url"/>. Provided for configuration compatibility.
    /// </summary>
    public Uri Destination
    {
        get => Url;
        set => Url = value;
    }

    /// <summary>
    /// A whitelist of event types to deliver.
    /// </summary>
    public ICollection<WebhookEventFilter> Filters { get; set; } = new List<WebhookEventFilter>();

    /// <summary>
    /// Alias for <see cref="Filters"/>. Provided for configuration compatibility.
    /// </summary>
    public ICollection<WebhookEventFilter> Subscriptions
    {
        get => Filters;
        set => Filters = value;
    }

    /// <summary>
    /// Optional dispatcher name to route this sink through a specific transport.
    /// </summary>
    public string? Dispatcher { get; set; }
}