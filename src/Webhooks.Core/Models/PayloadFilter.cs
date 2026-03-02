namespace Webhooks.Core;

public record PayloadPredicate(string Selector, string ExpectedValue);

public record PayloadFilter(string Selector, string ExpectedValue);