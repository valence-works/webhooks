namespace Webhooks.Core;

/// <summary>
/// Represents a webhook events source emitting events your application can receive
/// </summary>
public class WebhookSource
{
    /// A unique identifier for this sink.
    public string Id { get; set; } = default!;
    
    /// A friendly name for this endpoint.
    public string Name { get; set; }

    /// <summary>
    /// The origin of the webhook source.
    /// </summary>
    /// <example>https://github.com</example>
    public string Origin { get; set; } = default!;
    
    /// A list of event types that can be handled.
    public HashSet<WebhookSourceEventType> EventTypes { get; set; } = new();
}