namespace Radiate.Client.Domain.Templates.Panels;

public record CardPanel : Panel
{
    public Type Content { get; init; }
    public Dictionary<string, object> Props { get; init; } = new();
}
