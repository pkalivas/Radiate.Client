using Radiate.Client.Services.Store.Models;
using Radiate.Engines.Schema;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class RunSelectors
{
    public static readonly ISelectorWithoutProps<RootState, RunModel> SelectCurrentRun = Selectors
        .CreateSelector<RootState, RunModel>(state => state.Runs.TryGetValue(state.CurrentRunId, out var run) ? run : new RunModel());
    
    public static readonly ISelectorWithoutProps<RootState, RunInputsModel> SelectCurrentRunInputs = Selectors
        .CreateSelector<RootState, RunModel, RunInputsModel>(SelectCurrentRun, run => run.Inputs);
    
    public static readonly ISelectorWithoutProps<RootState, MetricsModel> SelectCurrentMetrics = Selectors
        .CreateSelector<RootState, RunModel, MetricsModel>(SelectCurrentRun, run => new MetricsModel
        {
            Index = run.Scores.Count,
            ScoresList = run.Scores,
            Fitness = run.Outputs.Metrics.Get(MetricNames.FitnessDistribution),
            GenomeSize = run.Outputs.Metrics.Get(MetricNames.GenomeSizeDistribution),
            PopulationAge = run.Outputs.Metrics.Get(MetricNames.AgeDistribution),
            Scores = run.Outputs.Metrics.Get(MetricNames.Score),
            IsRunning = run.IsRunning,
            IsPaused = run.IsPaused,
            IsCompleted = run.IsPaused
        });
}