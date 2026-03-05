using JetBrains.Annotations;
using Microsoft.Extensions.Options;

namespace Webhooks.Core.Options;

/// <summary>
/// Applies a configuration action to <see cref="WebhookSinksOptions"/>.
/// </summary>
[UsedImplicitly]
public sealed class ConfigureWebhookSinksOptions(Action<WebhookSinksOptions>? action) : ConfigureOptions<WebhookSinksOptions>(action);
