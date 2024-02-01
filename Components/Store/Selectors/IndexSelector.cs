using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;
using Radiate.Engines.Schema;

namespace Radiate.Client.Components.Store.Selectors;

public class IndexState
{
    public int Index { get; set; }
    
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