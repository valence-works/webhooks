using Microsoft.Extensions.DependencyInjection;
using Webhooks.Core;

namespace Webhooks.Core.Tests.Validation;

public class CoordinatorResolutionValidationTests
{
    [Fact]
    public void AddWebhooksCore_Registers_DispatcherCoordinator_And_DefaultDispatcher()
    {
        var services = new ServiceCollection();
        services.AddLogging();
        services.AddWebhooksCore();

        using var serviceProvider = services.BuildServiceProvider();

        var coordinator = serviceProvider.GetService<IDispatcherInvocationCoordinator>();
        var dispatchers = serviceProvider.GetServices<IWebhookDispatcher>();

        Assert.NotNull(coordinator);
        Assert.NotEmpty(dispatchers);
    }
}
