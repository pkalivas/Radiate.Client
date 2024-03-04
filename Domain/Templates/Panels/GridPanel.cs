namespace Radiate.Client.Domain.Templates.Panels;

public class GridPanel : Panel
{
    public List<GridItem> Items { get; set; } = new();

    public override List<IPanel> ChildPanels => Items.Select(i => i.Panel).ToList();

    public class GridItem
    {
        public int ColSpan { get; init; }
        public IPanel Panel { get; init; }
    }
}
