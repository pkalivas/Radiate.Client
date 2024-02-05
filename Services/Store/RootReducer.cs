using Radiate.Client.Services.Store.Actions;
using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Shared;
using Radiate.Engines.Schema;
using Reflow.Reducers;

namespace Radiate.Client.Services.Store;

public static class RootReducer
{
    public static IEnumerable<On<RootState>> CreateReducers() => new List<On<RootState>>
    {
        Reducer.On<NavigateToRunAction, RootState>((state, action) => state with { CurrentRunId = action.RunId }),
        Reducer.On<RunCreatedAction, RootState>(AddRun),
        Reducer.On<AddRunOutputAction, RootState>(AddOutput),
        Reducer.On<EngineStoppedAction, RootState>(RunCompleted),
        Reducer.On<StartEngineAction, RootState>(StartEngine),
        Reducer.On<PauseEngineRunAction, RootState>(PauseEngine),
        Reducer.On<ResumeEngineRunAction, RootState>(ResumeEngine),
        Reducer.On<SetEngineTreeExpandedAction, RootState>(SetTreeExpansions),
        Reducer.On<LayoutChangedAction, RootState>(LayoutChanged),
        Reducer.On<SetCurrentImageAction, RootState>(UpdateCurrentImage),
        Reducer.On<CancelEngineRunAction, RootState>(CancelEngine),
        Reducer.On<SetSelectedMetricsAction, RootState>(SetSelectedMetrics),
        Reducer.On<SetRunInputsAction, RootState>(SetRunInputs),
        Reducer.On<SetTargetImageAction, RootState>(SetTargetImage)
    };
    
    private static RootState AddOutput(RootState state, AddRunOutputAction action)
    {
        var outputs = action.EngineOutputs;
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with
        {
            Outputs = outputs,
            Metrics = MetricMappers.GetMetricValues(outputs.Metrics)
                .Select(model => (model, state.Runs[state.CurrentRunId].Metrics.GetValueOrDefault(model.Name, new MetricValueModel())))
                .Select(pair =>
                {
                    if (pair.model.MetricType is not (MetricTypes.Description or MetricTypes.Distribution))
                    {
                        return pair.model with
                        {
                            Distribution = pair.Item2.Distribution
                                .Concat(new []{ pair.model.Value })
                                .ToArray(),
                        };
                    }
                    
                    return pair.model with { };
                })
                .ToDictionary(key => key.Name, value => value),
            Scores = state.Runs[state.CurrentRunId].Scores
                .Concat(new[]
                {
                    outputs.Metrics.Get(MetricNames.Score).Statistics.LastValue
                })
                .ToList(),
        };

        return state with { Runs = state.Runs };
    }
    
    private static RootState AddRun(RootState state, RunCreatedAction action)
    {
        state.Runs[action.Run.RunId] = action.Run with { Index = state.Runs.Count };
        return state with { Runs = state.Runs };
    }
    
    private static RootState RunCompleted(RootState state, EngineStoppedAction action)
    {
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with
        {
            IsRunning = false,
            IsPaused = false,
            IsCompleted = true,
            EndTime = DateTime.Now
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState StartEngine(RootState state, StartEngineAction action)
    {
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with
        {
            IsRunning = true,
            IsPaused = false,
            IsCompleted = false,
            StartTime = DateTime.Now
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState ResumeEngine(RootState state, ResumeEngineRunAction action)
    {
        state.Runs[action.RunId] = state.Runs[action.RunId] with
        {
            IsPaused = false,
            IsRunning = true,
            IsCompleted = false
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState PauseEngine(RootState state, PauseEngineRunAction action)
    {
        state.Runs[action.RunId] = state.Runs[action.RunId] with
        {
            IsPaused = true,
            IsRunning = true,
            IsCompleted = false
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState CancelEngine(RootState state, CancelEngineRunAction action)
    {
        state.Runs[action.RunId] = state.Runs[action.RunId] with
        {
            IsPaused = false,
            IsRunning = false,
            IsCompleted = true
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState SetTreeExpansions(RootState state, SetEngineTreeExpandedAction action)
    {
        state.UiModel.EngineStateExpanded[action.RunId] = action.Expanded;
        return state with { UiModel = state.UiModel };
    }
    
    private static RootState LayoutChanged(RootState state, LayoutChangedAction action) => state with
    {
        UiModel = state.UiModel with { IsSidebarOpen = action.IsSidebarOpen }
    };

    private static RootState UpdateCurrentImage(RootState state, SetCurrentImageAction action)
    {
        var run = state.Runs[action.RunId];
        state.Runs[action.RunId] = run with 
        {
            Outputs = run.Outputs with
            {
                ImageOutput = run.Outputs.ImageOutput with
                {
                    Image = action.Image
                }
            }
        };

        return state with { Runs = state.Runs };
    }
    
    private static RootState SetSelectedMetrics(RootState state, SetSelectedMetricsAction action)
    {
        var run = state.Runs[state.CurrentRunId];
        state.Runs[state.CurrentRunId] = run with
        {
            SelectedMetrics = action.Metrics.ToHashSet()
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState SetRunInputs(RootState state, SetRunInputsAction action)
    {
        var run = state.Runs[action.RunId];
        state.Runs[action.RunId] = run with
        {
            Inputs = action.Inputs
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState SetTargetImage(RootState state, SetTargetImageAction action)
    {
        var run = state.Runs[state.CurrentRunId];
        state.Runs[state.CurrentRunId] = run with
        {
            Inputs = run.Inputs with
            {
                ImageInputs = run.Inputs.ImageInputs with
                {
                    TargetImage = action.Image
                }
            }
        };
        return state with { Runs = state.Runs };
    }

}