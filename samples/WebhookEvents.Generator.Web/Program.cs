using WebhookEvents.Generator.Web.HostedServices;
using Webhooks.Core;
using Webhooks.Core.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var services = builder.Services;

services
    .AddWebhooksCore()
    .AddWebhooksBackgroundProcessor()
    .AddSingleton(TimeProvider.System)
    .AddHostedService<EventGenerator>()
    ;

services.Configure<WebhookSinksOptions>(options => configuration.GetSection("Webhooks").Bind(options));
services.Configure<WebhookBroadcasterOptions>(options => options.UseBackgroundProcessorBroadcasterStrategy());
var app = builder.Build();

app.UseHttpsRedirection();
app.Run();