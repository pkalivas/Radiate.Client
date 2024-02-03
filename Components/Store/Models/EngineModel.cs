using Radiate.Engines.Entities;

namespace Radiate.Client.Components.Store.Models;

public record EngineModel
{
    public Guid RunId { get; init; }
    public HashSet<TreeItemData<EngineState>> TreeItems { get; init; } = new();
    public Dictionary<string, bool> Expanded { get; init; } = new();
    public RunInputsModel Inputs { get; set; } = new();
    public EngineState? CurrentEngineState { get; set; }
}
