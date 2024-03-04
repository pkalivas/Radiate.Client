using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record StandardRunUiProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsLoading { get; init; } = true;
    public IRunUITemplate? UiTemplate { get; init; } = default!;
    public HashSet<TreeItemData<RunPanelState>> Panels { get; init; } = new();
}