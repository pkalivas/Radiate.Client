namespace Radiate.Client.Domain.Templates.Panels;

public class ToolbarPanel : Panel
{
    public Type Content { get; set; }
    public List<string> Actions { get; set; }
}