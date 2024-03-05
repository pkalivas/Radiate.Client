namespace Radiate.Client.Domain.Templates.Panels;

public interface IPanel
{
    Guid Id { get; }
    string Title { get; }
    List<IPanel> ChildPanels { get; }
}

public abstract class Panel : IPanel
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string Title { get; init; } = string.Empty;
    public virtual List<IPanel> ChildPanels { get; init; } = new();
}
