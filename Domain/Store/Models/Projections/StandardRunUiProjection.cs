using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record StandardRunUiProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsLoading { get; init; } = true;
    public IRunUITemplate? UiTemplate { get; init; } = default!;
    public PanelState[] OrderedPanels { get; init; } = Array.Empty<PanelState>();
    public TreeItemData<PanelState, Guid>[] PanelStates { get; init; } = Array.Empty<TreeItemData<PanelState, Guid>>();
}