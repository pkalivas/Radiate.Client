using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.Projections;
using Radiate.Client.Services.Store.Models.States;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class RunSelectors
{
    public static ISelector<RootState, Guid> SelectCurrentRunId => Selectors
        .Create<RootState, Guid>(state => state.CurrentRunId);
    
    public static ISelector<RootState, RunState> SelectRun => Selectors
        .Create<RootState, RunState>(state => state.Runs.TryGetValue(state.CurrentRunId, out var run) ? run : new RunState());
    
    public static ISelector<RootState, InputsPanelModelProjection> SelectInputsModel => Selectors
        .Create<RootState, RunState, InputsPanelModelProjection>(SelectRun, run => new InputsPanelModelProjection
        {
            RunId = run.RunId,
            Inputs = run.Inputs,
            IsReadonly = run.IsRunning || run.IsPaused || run.IsCompleted
        });
    
    public static readonly ISelector<RootState, MetricDataGridPanelProjection> SelectMetricDataGridPanelModel = Selectors
        .Create<RootState, RunState, MetricDataGridPanelProjection>(SelectRun, run => new MetricDataGridPanelProjection
        {
            RunId = run.RunId,
            Values = run.Metrics.Values.ToList()
        });
    
    public static ISelector<RootState, MetricSummaryChartPanelProjection> SelectMetricSummaryChartPanelModel(string metricName) => Selectors
        .Create<RootState, RunState, MetricSummaryChartPanelProjection>(SelectRun, run => new MetricSummaryChartPanelProjection
        {
            RunId = run.RunId,
            MetricName = metricName,
            Value = run.Metrics.GetValueOrDefault(metricName, new MetricValueModel())
        });
    
    
}