namespace Radiate.Client.Domain.Templates.Panels;

public class AccordionPanel : Panel
{
    public List<AccordionPanelItem> Panels { get; init; } = new();
}

public class AccordionPanelItem : Panel
{
    public bool Expanded { get; init; } = false;
    public Type Content { get; init; } = typeof(Panel);
}