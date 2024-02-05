using Radiate.Client.Services.Store.Models.States;
using Radiate.Engines.Entities;

namespace Radiate.Client.Services.Store.Models.Projections;

public record EngineTreePanelProjection
{
    public Guid RunId { get; init; }
    public HashSet<TreeItemData<EngineState>> TreeItems { get; init; } = new();
    public Dictionary<string, bool> Expanded { get; init; } = new();
    public RunInputsState Inputs { get; set; } = new();
    public EngineState? CurrentEngineState { get; set; }
}
