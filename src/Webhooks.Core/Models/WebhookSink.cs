using JetBrains.Annotations;

namespace Webhooks.Core;

/// Represents a webhook events sink interested in a given set of events. 
[UsedImplicitly]
public class WebhookSink
{
    /// A unique identifier for this sink.
    public string Id { get; set; } = default!;

    public string SinkId
    {
        get => Id;
        set => Id = value;
    }

    /// A friendly name for this endpoint.
    public string? Name { get; set; }

    /// The URL to send the webhook event to.
    public Uri Url { get; set; } = default!;

    public Uri Destination
    {
        get => Url;
        set => Url = value;
    }

    /// A whitelist of event types to deliver.
    public ICollection<WebhookEventFilter> Filters { get; set; } = new List<WebhookEventFilter>();

    public ICollection<WebhookEventFilter> Subscriptions
    {
        get => Filters;
        set => Filters = value;
    }

    public string? Dispatcher { get; set; }
}