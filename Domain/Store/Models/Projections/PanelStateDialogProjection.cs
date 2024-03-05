using Radiate.Client.Domain.Templates.Panels;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record PanelStateDialogProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public List<PanelStateModel> Panels { get; init; } = new();
}

public record PanelStateModel 
{
    public Guid PanelId { get; init; } = Guid.Empty;
    public string PanelType { get; init; } = string.Empty;
    public string PanelName { get; init; } = string.Empty;
    public bool IsVisible { get; init; } = true;
    public bool IsExpanded { get; init; } = true;
    public IPanel Panel { get; init; } = default!;
}