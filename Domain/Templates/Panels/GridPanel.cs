namespace Radiate.Client.Domain.Templates.Panels;

public class GridPanel : Panel
{
    public List<GridItem> Items { get; set; } = new();

    public override List<IPanel> ChildPanels => Items.Cast<IPanel>().ToList();

    public class GridItem : Panel
    {
        public int ColSpan { get; init; }
        public IPanel Panel { get; init; }

        public override List<IPanel> ChildPanels => new List<IPanel> { Panel };
    }
}
