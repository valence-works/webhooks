namespace Webhooks.Core;

public class WebhookEventFilter
{
    public string EventType { get; set; } = default!;
    public ICollection<PayloadFilter> PayloadFilters { get; set; } = new List<PayloadFilter>();
}