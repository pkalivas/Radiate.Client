using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.Projections;

namespace Radiate.Client.Domain.Store.Models.States.Outputs;

public record EngineStateOutputs
{
    public ImmutableArray<EngineOutput> EngineOutputs { get; init; } = ImmutableArray<EngineOutput>.Empty;
}

public record EngineOutput : ITreeItem<int>
{
    public int Index { get; init; }
    public string Name { get; init; } = "";
    public string EngineId { get; init; } = "";
    public string State { get; init; } = "";
    public IEnumerable<int> Children { get; init; } = [];
    public ImmutableDictionary<string, MetricValueModel> Metrics { get; init; } = ImmutableDictionary<string, MetricValueModel>.Empty;

    public bool IsCyclic() => true;
}