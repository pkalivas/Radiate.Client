namespace Radiate.Client.Domain.Templates.Panels;

public record ToolbarPanel : BoundedPaperPanel
{
    public List<string> Actions { get; init; } = new();
}