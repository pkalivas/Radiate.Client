using Radiate.Client.Domain.Store.Models.States;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record StandardRunUiProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsLoading { get; init; } = true;
    public TreeItem<PanelState, Guid>[] PanelStates { get; init; } = Array.Empty<TreeItem<PanelState, Guid>>();
}