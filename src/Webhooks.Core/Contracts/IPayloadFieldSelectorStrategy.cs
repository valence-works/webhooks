using System.Text.Json;

namespace Webhooks.Core;

/// <summary>
/// Extracts a field value from a JSON payload using a selector expression.
/// </summary>
public interface IPayloadFieldSelectorStrategy
{
    /// <summary>
    /// Attempts to select a value from the payload using the specified selector.
    /// </summary>
    /// <param name="payload">The JSON payload to query.</param>
    /// <param name="selector">The selector expression (e.g. a JsonPath).</param>
    /// <param name="selectedValue">The extracted value, or <c>null</c> if not found.</param>
    /// <returns><c>true</c> if a value was found; otherwise <c>false</c>.</returns>
    bool TrySelect(JsonElement payload, string selector, out string? selectedValue);
}
