namespace Webhooks.Core;

public class WebhookSourceEventType
{
    public string EventType { get; set; } = default!;
    
    public Type? PayloadType { get; set; }
    public WebhookActivityBinding? ActivityBinding { get; set; }
}