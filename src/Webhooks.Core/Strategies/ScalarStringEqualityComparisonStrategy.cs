namespace Webhooks.Core.Strategies;

/// <summary>
/// Compares payload values using ordinal string equality.
/// </summary>
public sealed class ScalarStringEqualityComparisonStrategy : IPayloadValueComparisonStrategy
{
    public bool IsMatch(string? actualValue, string expectedValue)
    {
        return string.Equals(actualValue, expectedValue, StringComparison.Ordinal);
    }
}
