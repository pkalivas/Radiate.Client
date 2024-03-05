namespace Radiate.Client.Domain.Templates.Panels;

public record TabPanel : Panel
{
    public override List<IPanel> ChildPanels { get; init; } = new();
}
