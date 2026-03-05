namespace Webhooks.Core;

/// <summary>
/// A filter that matches a payload field value against an expected value.
/// </summary>
public sealed record PayloadFilter(string Selector, string ExpectedValue);