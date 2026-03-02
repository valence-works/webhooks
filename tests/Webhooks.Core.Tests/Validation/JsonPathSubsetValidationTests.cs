using System.Text.Json;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Validation;

public class JsonPathSubsetValidationTests
{
    [Fact]
    public void TrySelect_Returns_True_For_Restricted_JsonPath_Subset()
    {
        var strategy = new JsonPathPayloadFieldSelectorStrategy();
        var payload = JsonSerializer.SerializeToElement(new { customer = new { id = "123" } });

        var matched = strategy.TrySelect(payload, "$.customer.id", out var value);

        Assert.True(matched);
        Assert.Equal("123", value);
    }

    [Fact]
    public void TrySelect_Returns_False_For_Invalid_Selector()
    {
        var strategy = new JsonPathPayloadFieldSelectorStrategy();
        var payload = JsonSerializer.SerializeToElement(new { customer = new { id = "123" } });

        var matched = strategy.TrySelect(payload, "customer.id", out _);

        Assert.False(matched);
    }
}
