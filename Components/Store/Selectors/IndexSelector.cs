using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Engines.Schema;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Selectors;

public class IndexState : ICopy<IndexState>
{
    public int Index { get; set; }

    public IndexState Copy() => new IndexState
    {
        Index = Index
    };

    public override bool Equals(object? obj) => obj is IndexState state && Index == state.Index;
}

public static class IndexSelector
{
    public static IState<IndexState> Select(StateStore store) => store.Select<AppFeature>()
        .SelectState(feature => new IndexState
        {
            Index = (int) (feature.EngineOutputs?.Metrics?.Get(MetricNames.Index)?.Statistics?.LastValue ?? 0)
        });
}