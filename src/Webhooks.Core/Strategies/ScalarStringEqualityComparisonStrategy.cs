namespace Webhooks.Core.Strategies;

public class ScalarStringEqualityComparisonStrategy : IPayloadValueComparisonStrategy
{
    public bool IsMatch(string? actualValue, string expectedValue)
    {
        return string.Equals(actualValue, expectedValue, StringComparison.Ordinal);
    }
}
