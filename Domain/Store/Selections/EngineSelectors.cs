using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Mappers;
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
    
    public static readonly ISelector<RootState, EngineTreePanelProjection> SelectEngineTreePanelModel = Selectors
        .Create<RootState, UiState, RunState, EngineTreePanelProjection>(UiSelectors.SelectUiState, 
            RunSelectors.SelectRun, (ui, run) =>
            {
                var expanded = ui.EngineStateExpanded.TryGetValue(run.RunId, out var engineTree)
                    ? engineTree
                    : run.Outputs.EngineStates.Keys.ToDictionary(key => key, _ => true);
                
                return new EngineTreePanelProjection
                {
                    RunId = run.RunId,
                    TreeItems = TreeItemMapper.GetItems(run.Outputs.EngineStates, expanded),
                    Expanded = expanded,
                    Inputs = run.Inputs,
                    CurrentEngineState = run?.Outputs?.EngineStates.FirstOrDefault().Value
                };
            });
    
    public static readonly ISelector<RootState, PanelToolbarProjection> SelectPanelToolbarModel = Selectors
        .Create<RootState, RunControlPanelProjection, EngineTreePanelProjection, PanelToolbarProjection>(SelectRunControlPanelModel, SelectEngineTreePanelModel,
            (control, model) => new PanelToolbarProjection
            {
                IsRunning = control.IsRunning,
                IsPaused = control.IsPaused,
                IsComplete = control.IsCompleted,
                Index = (int)(model.CurrentEngineState?.Metrics.Get(MetricNames.Run)?.Statistics?.Sum ?? 0),
                Score = model.CurrentEngineState?.Metrics.Get(MetricNames.Score)?.Statistics?.LastValue ?? 0,
                EngineState = model.CurrentEngineState?.State ?? EngineStateTypes.Pending,
                Duration = TimeSpan.FromMilliseconds(model?.CurrentEngineState?.Metrics.Get(MetricNames.Time)?.Time?.Sum ?? 0)
            });
    
}