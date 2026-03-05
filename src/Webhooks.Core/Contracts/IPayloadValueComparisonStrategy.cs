namespace Webhooks.Core;

/// <summary>
/// Compares an extracted payload value against an expected value for subscription filtering.
/// </summary>
public interface IPayloadValueComparisonStrategy
{
    /// <summary>
    /// Determines whether the actual value matches the expected value.
    /// </summary>
    /// <param name="actualValue">The value extracted from the payload.</param>
    /// <param name="expectedValue">The expected value from the subscription filter.</param>
    /// <returns><c>true</c> if the values match; otherwise <c>false</c>.</returns>
    bool IsMatch(string? actualValue, string expectedValue);
}
