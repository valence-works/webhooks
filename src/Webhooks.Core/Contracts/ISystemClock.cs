namespace Webhooks.Core;

/// <summary>
/// Provides access to the current UTC time.
/// </summary>
public interface ISystemClock
{
    /// <summary>
    /// Gets the current UTC date and time.
    /// </summary>
    DateTimeOffset UtcNow { get; }
}