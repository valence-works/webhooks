namespace Webhooks.Core;

public interface IPayloadValueComparisonStrategy
{
    bool IsMatch(string? actualValue, string expectedValue);
}
