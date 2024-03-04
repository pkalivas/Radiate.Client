namespace Radiate.Client.Domain.Templates.Panels;

public class AccordionPanel : Panel
{
    public Type? Toolbar { get; init; } = null;
    public override List<IPanel> ChildPanels { get; init; } = new();
}

public class AccordionPanelItem : Panel
{
    public bool Expanded { get; init; } = false;
    public Type Content { get; init; } = typeof(Panel);
}