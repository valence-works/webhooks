namespace Webhooks.Core;

/// <summary>
/// A predicate that matches a payload field value against an expected value.
/// </summary>
public sealed record PayloadPredicate(string Selector, string ExpectedValue);
