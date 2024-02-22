using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record EngineTreePanelProjection
{
    public Guid RunId { get; init; }
    public HashSet<TreeItemData<EngineState>> TreeItems { get; init; } = new();
    public IImmutableDictionary<string, bool> Expanded { get; init; } = new Dictionary<string, bool>().ToImmutableDictionary();
    public RunInputsState Inputs { get; set; } = new();
    public EngineState? CurrentEngineState { get; set; }
}
