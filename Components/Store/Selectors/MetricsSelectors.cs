using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Engines.Entities;
using Radiate.Engines.Schema;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Selectors;

public record MetricsState : ICopy<MetricsState>
{
    public float Score => Scores?.Statistics?.LastValue ?? 0;
    public int Index { get; init; }
    public List<float> ScoresList { get; init; } = new();
    public MetricValue Scores { get; init; } = new();
    public MetricValue Fitness { get; init; } = new();
    public MetricValue GenomeSize { get; init; } = new();
    public MetricValue PopulationAge { get; init; } = new();
    
    public MetricsState Copy() => new()
    {
        Index = Index,
        Fitness = Fitness,
        GenomeSize = GenomeSize,
        PopulationAge = PopulationAge,
        Scores = Scores
    };
}


public static class MetricsSelectors
{
    public static IState<MetricsState> Select(StateStore store) =>
        store.GetState<RootFeature>()
            .SelectState(feature =>
            {
                if (!feature.Runs.ContainsKey(feature.CurrentRunId))
                {
                    return new MetricsState();
                }
                
                var lastRun = feature.Runs[feature.CurrentRunId];
                
                return new MetricsState
                {
                    Index = lastRun.Scores.Count,
                    ScoresList = feature.Runs[feature.CurrentRunId].Scores,
                    Fitness = lastRun.Outputs.Metrics.Get(MetricNames.FitnessDistribution),
                    GenomeSize = lastRun.Outputs.Metrics.Get(MetricNames.GenomeSizeDistribution),
                    PopulationAge = lastRun.Outputs.Metrics.Get(MetricNames.AgeDistribution),
                    Scores = lastRun.Outputs.Metrics.Get(MetricNames.Score)
                };
            });

    public static readonly ISelectorWithoutProps<RootFeature, MetricsState> SelectCurrentMetrics =
        Reflow.Selectors.Selectors.CreateSelector<RootFeature, MetricsState>(state =>
        {
            if (!state.Runs.ContainsKey(state.CurrentRunId))
            {
                return new MetricsState();
            }

            var lastRun = state.Runs[state.CurrentRunId];

            return new MetricsState
            {
                Index = lastRun.Scores.Count,
                ScoresList = state.Runs[state.CurrentRunId].Scores,
                Fitness = lastRun.Outputs.Metrics.Get(MetricNames.FitnessDistribution),
                GenomeSize = lastRun.Outputs.Metrics.Get(MetricNames.GenomeSizeDistribution),
                PopulationAge = lastRun.Outputs.Metrics.Get(MetricNames.AgeDistribution),
                Scores = lastRun.Outputs.Metrics.Get(MetricNames.Score)
            };
        });
}