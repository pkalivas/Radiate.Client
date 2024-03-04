using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Schema;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class EngineSelectors
{
    public static readonly ISelector<RootState, RunControlPanelProjection> SelectRunControlPanelModel = Selectors
        .Create<RootState, RunState, RunControlPanelProjection>(RunSelectors.SelectRun, run => new RunControlPanelProjection
        {
            RunId = run.RunId,
            ModelType = run.Inputs.ModelType,
            IsRunning = run.IsRunning,
            IsPaused = run.IsPaused,
            IsCompleted = run.IsCompleted,
            NeedsImageUpload = run.Inputs is {ModelType: "Image", ImageInputs.TargetImage.IsEmpty: true},
            StartTime = run.StartTime,
            EndTime = run.IsCompleted ? run.EndTime : DateTime.Now,
            Inputs = run.Inputs,
            Score = run.Outputs.Metrics.TryGetValue(MetricNames.Score, out var metric) ? metric.Value : 0f,
            ElapsedTime = run.Outputs.Metrics.TryGetValue(MetricNames.Time, out var timeMetric) ? timeMetric.Total : TimeSpan.Zero,
            Index = run.Outputs.Metrics.TryGetValue(MetricNames.Index, out var indexMetric) ? (int)indexMetric.Value : 0
        });
    
    public static readonly ISelector<RootState, RunSimpleStatsPanelProjection> SelectRunSimpleStatsPanelModel = Selectors
        .Create<RootState, RunState, RunSimpleStatsPanelProjection>(RunSelectors.SelectRun, run => new RunSimpleStatsPanelProjection
        {
            RunId = run.RunId,
            StartTime = run.StartTime,
            EndTime = run.IsCompleted ? run.EndTime : DateTime.Now,
            ElapsedTime = run.IsCompleted ? run.EndTime - run.StartTime : DateTime.Now - run.StartTime,
            Score = run.Outputs.Metrics.TryGetValue(MetricNames.Score, out var metric) ? metric.Value : 0f,
            Index = run.Outputs.Metrics.TryGetValue(MetricNames.Index, out var indexMetric) ? (int)indexMetric.Value : 0
        });
    
    public static readonly ISelector<RootState, PanelToolbarProjection> SelectPanelToolbarModel = Selectors
        .Create<RootState, RunControlPanelProjection, PanelToolbarProjection>(SelectRunControlPanelModel, control => new PanelToolbarProjection
            {
                RunId = control.RunId,
                ModelType = control.Inputs.ModelType,
                IsRunning = control.IsRunning,
                IsPaused = control.IsPaused,
                IsComplete = control.IsCompleted,
            });
    
    public static readonly ISelector<RootState, EngineStateTableModelProjection> SelectEngineStateTablePanelModel = Selectors
        .Create<RootState, RunState, EngineStateTableModelProjection>(RunSelectors.SelectRun, run => new EngineStateTableModelProjection
        {
            RunId = run.RunId,
            IsComplete = run.IsCompleted,
            EngineOutputs = run.Outputs.EngineStateOutputs.EngineOutputs.ToArray()
        });
    
}