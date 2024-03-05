namespace Radiate.Client.Domain.Templates.Panels;

public record AccordionPanel : Panel
{
    public Type? Toolbar { get; init; } = null;
    public List<AccordionPanelItem> Items { get; init; } = new();
    public override List<IPanel> ChildPanels => Items.Cast<IPanel>().ToList();
}

public record AccordionPanelItem : Panel
{
    public bool Expanded { get; init; } = false;
    public Type Content { get; init; } = typeof(Panel);
}