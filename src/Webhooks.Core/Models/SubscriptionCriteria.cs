namespace Webhooks.Core;

/// <summary>
/// Base class for subscription criteria that match events by type and payload predicates.
/// </summary>
public class SubscriptionCriteria
{
    /// <summary>
    /// The event type this subscription matches.
    /// </summary>
    public string EventType { get; set; } = default!;

    /// <summary>
    /// The collection of payload predicates to evaluate.
    /// </summary>
    public ICollection<PayloadFilter> PayloadPredicates { get; set; } = new List<PayloadFilter>();

    /// <summary>
    /// The mode used to combine payload predicate results.
    /// </summary>
    public PayloadMatchingMode? PayloadMatchingMode { get; set; }

    /// <summary>
    /// Alias for <see cref="PayloadPredicates"/>.
    /// </summary>
    public ICollection<PayloadFilter> PayloadFilters
    {
        get => PayloadPredicates;
        set => PayloadPredicates = value;
    }
}
