using Webhooks.Core.Options;
using Webhooks.Core.Strategies;

namespace Webhooks.Core.Tests.Validation;

public sealed class EventTypeMatcherStrategyValidationTests
{
    [Fact]
    public void Validate_Succeeds_With_WildcardEventTypeMatcherStrategy()
    {
        var validator = new ConfigureWebhookEventBroadcasterOptions();
        var options = new WebhookBroadcasterOptions
        {
            EventTypeMatcherStrategy = typeof(WildcardEventTypeMatcherStrategy)
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_Succeeds_With_ExactEventTypeMatcherStrategy()
    {
        var validator = new ConfigureWebhookEventBroadcasterOptions();
        var options = new WebhookBroadcasterOptions
        {
            EventTypeMatcherStrategy = typeof(ExactEventTypeMatcherStrategy)
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Succeeded);
    }

    [Fact]
    public void Validate_Fails_When_EventTypeMatcherStrategy_Is_Not_Assignable()
    {
        var validator = new ConfigureWebhookEventBroadcasterOptions();
        var options = new WebhookBroadcasterOptions
        {
            EventTypeMatcherStrategy = typeof(string)
        };

        var result = validator.Validate(null, options);

        Assert.True(result.Failed);
    }

    [Fact]
    public void UseEventTypeMatcher_Generic_Sets_Strategy_Type()
    {
        var options = new WebhookBroadcasterOptions();

        options.UseEventTypeMatcher<ExactEventTypeMatcherStrategy>();

        Assert.Equal(typeof(ExactEventTypeMatcherStrategy), options.EventTypeMatcherStrategy);
    }

    [Fact]
    public void UseEventTypeMatcher_Runtime_Sets_Strategy_Type()
    {
        var options = new WebhookBroadcasterOptions();

        options.UseEventTypeMatcher(typeof(ExactEventTypeMatcherStrategy));

        Assert.Equal(typeof(ExactEventTypeMatcherStrategy), options.EventTypeMatcherStrategy);
    }

    [Fact]
    public void UseEventTypeMatcher_Runtime_Throws_For_Invalid_Type()
    {
        var options = new WebhookBroadcasterOptions();

        Assert.Throws<ArgumentException>(() => options.UseEventTypeMatcher(typeof(string)));
    }

    [Fact]
    public void UseWildcardEventTypeMatcher_Sets_WildcardStrategy()
    {
        var options = new WebhookBroadcasterOptions();
        options.EventTypeMatcherStrategy = typeof(ExactEventTypeMatcherStrategy); // change from default

        options.UseWildcardEventTypeMatcher();

        Assert.Equal(typeof(WildcardEventTypeMatcherStrategy), options.EventTypeMatcherStrategy);
    }

    [Fact]
    public void UseExactEventTypeMatcher_Sets_ExactStrategy()
    {
        var options = new WebhookBroadcasterOptions();

        options.UseExactEventTypeMatcher();

        Assert.Equal(typeof(ExactEventTypeMatcherStrategy), options.EventTypeMatcherStrategy);
    }

    [Fact]
    public void Default_EventTypeMatcherStrategy_Is_WildcardMatcher()
    {
        var options = new WebhookBroadcasterOptions();

        Assert.Equal(typeof(WildcardEventTypeMatcherStrategy), options.EventTypeMatcherStrategy);
    }
}
