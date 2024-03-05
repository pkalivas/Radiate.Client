namespace Radiate.Client.Domain.Templates.Panels;

public record PaperPanel : Panel
{
    public bool DisplayHeader { get; init; } = true;
    public Type Content { get; init; }
    public Dictionary<string, object> Props { get; init; } = new();
}

public record BoundedPaperPanel : PaperPanel
{
    public int Height { get; init; } = 250;
}