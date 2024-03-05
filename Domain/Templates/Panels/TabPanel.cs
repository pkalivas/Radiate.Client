namespace Radiate.Client.Domain.Templates.Panels;

public class TabPanel : Panel
{
    public override List<IPanel> ChildPanels { get; init; } = new();
}
