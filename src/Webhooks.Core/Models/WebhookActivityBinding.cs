namespace Webhooks.Core;

public class WebhookActivityBinding
{
    public string TypeName { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string? Description { get; set; }
}