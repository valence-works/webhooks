namespace Webhooks.Core;

public enum PayloadMatchingMode
{
    And,
    Or
}

public class SubscriptionCriteria
{
    public string EventType { get; set; } = default!;
    public ICollection<PayloadFilter> PayloadPredicates { get; set; } = new List<PayloadFilter>();
    public PayloadMatchingMode? PayloadMatchingMode { get; set; }

    public ICollection<PayloadFilter> PayloadFilters
    {
        get => PayloadPredicates;
        set => PayloadPredicates = value;
    }
}

public class WebhookEventFilter : SubscriptionCriteria
{
}