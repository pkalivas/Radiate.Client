using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.Projections;
using Radiate.Engines.Schema;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class RunSelectors
{
    public static ISelector<RootState, Guid> SelectCurrentRunId => Selectors
        .Create<RootState, Guid>(state => state.CurrentRunId);
    
    public static ISelector<RootState, RunModel> SelectRun => Selectors
        .Create<RootState, RunModel>(state => state.Runs.TryGetValue(state.CurrentRunId, out var run) ? run : new RunModel());
    
    public static readonly ISelector<RootState, RunInputsModel> SelectCurrentRunInputs = Selectors
        .Create<RootState, RunModel, RunInputsModel>(SelectRun, run => run.Inputs);
    
    public static ISelector<RootState, InputsPanelModelProjection> SelectInputsModel => Selectors
        .Create<RootState, RunModel, InputsPanelModelProjection>(SelectRun, run => new InputsPanelModelProjection
        {
            RunId = run.RunId,
            Inputs = run.Inputs,
            IsReadonly = run.IsRunning || run.IsPaused || run.IsCompleted
        });
    
    public static readonly ISelector<RootState, MetricsModel> SelectCurrentMetrics = Selectors
        .Create<RootState, RunModel, MetricsModel>(SelectRun, run => new MetricsModel
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
    
    public static readonly ISelector<RootState, MetricDataGridPanelProjection> SelectMetricDataGridPanelModel = Selectors
        .Create<RootState, RunModel, MetricDataGridPanelProjection>(SelectRun, run => new MetricDataGridPanelProjection
        {
            RunId = run.RunId,
            SelectedMetrics = run.SelectedMetrics.Select(val => run.Metrics.GetValueOrDefault(val, new MetricValueModel())).ToHashSet(),
            Values = run.Metrics.Values.ToList()
        });
    
    public static ISelector<RootState, MetricSummaryChartPanelProjection> SelectMetricSummaryChartPanelModel(string metricName) => Selectors
        .Create<RootState, RunModel, MetricSummaryChartPanelProjection>(SelectRun, run => new MetricSummaryChartPanelProjection
        {
            RunId = run.RunId,
            MetricName = metricName,
            Value = run.Metrics.GetValueOrDefault(metricName, new MetricValueModel())
        });
    
    
}