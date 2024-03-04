namespace Radiate.Client.Domain.Templates.Panels;

public class ToolbarPanel : BoundedPaperPanel
{
    public List<string> Actions { get; init; } = new();
}