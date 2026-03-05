using Microsoft.Extensions.Options;

namespace Webhooks.Core.Options;

/// <summary>
/// Applies a configuration action to <see cref="BackgroundTaskProcessorOptions"/>.
/// </summary>
public sealed class ConfigureBackgroundTaskProcessorOptions(Action<BackgroundTaskProcessorOptions>? action) : ConfigureOptions<BackgroundTaskProcessorOptions>(action)
{
}
