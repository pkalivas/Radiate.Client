using Radiate.Client.Domain.Templates.Panels;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record PanelStateProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public Guid PanelId { get; init; } = Guid.NewGuid();
    public List<PanelStateProjection> Children { get; init; } = new();
    public IPanel Panel { get; init; } = default!;
    public bool IsVisible { get; init; } = false;
    public bool IsExpanded { get; init; } = false;

    public string Key => $"{RunId}_{PanelId}_{IsVisible}_{IsExpanded}";
}

public record PanelStateChild
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public Guid PanelId { get; init; } = Guid.NewGuid();
    public bool IsVisible { get; init; } = false;
    public bool IsExpanded { get; init; } = false;

    public string Key => $"{RunId}_{PanelId}_{IsVisible}_{IsExpanded}";
}