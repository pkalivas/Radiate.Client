namespace Radiate.Client.Domain.Templates.Panels;

public record GridPanel : Panel
{
    public List<GridItem> Items { get; init; } = new();

    public override List<IPanel> ChildPanels => Items.Cast<IPanel>().ToList();

    public record GridItem : Panel
    {
        public int ColSpan { get; init; }
        public bool IsVisible { get; init; } = true;
        public IPanel Panel { get; init; } = default!;

        public override List<IPanel> ChildPanels => [Panel];
    }
}
