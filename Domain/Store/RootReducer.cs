using Plotly.Blazor.Traces;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store;

public static class RootReducer
{
    public static IEnumerable<On<RootState>> CreateReducers() => new List<On<RootState>>
    {
        Reducer.On<NavigateToRunAction, RootState>(NavigateToRun),
        Reducer.On<RunCreatedAction, RootState>(AddRun),
        Reducer.On<SetRunOutputsAction, RootState>(AddOutput),
        Reducer.On<EngineStoppedAction, RootState>(RunCompleted),
        Reducer.On<StartEngineAction, RootState>(StartEngine),
        Reducer.On<PauseEngineRunAction, RootState>(PauseEngine),
        Reducer.On<ResumeEngineRunAction, RootState>(ResumeEngine),
        Reducer.On<SetEngineTreeExpandedAction, RootState>(SetTreeExpansions),
        Reducer.On<LayoutChangedAction, RootState>(LayoutChanged),
        Reducer.On<CancelEngineRunAction, RootState>(CancelEngine),
        Reducer.On<SetRunInputsAction, RootState>(SetRunInputs),
        Reducer.On<SetTargetImageAction, RootState>(SetTargetImage),
        Reducer.On<CopyRunAction, RootState>(CopyRun),
    };

    private static RootState NavigateToRun(RootState state, NavigateToRunAction action) => state with
    {
        CurrentRunId = action.RunId,
        UiState = state.UiState with
        {
            IsSidebarOpen = false
        }
    };
    
    private static RootState AddOutput(RootState state, SetRunOutputsAction action)
    {
        var outputs = action.EngineOutputs;
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with
        {
            Outputs = outputs.Last(),
            Scores = state.Runs[state.CurrentRunId].Scores
                .Concat(outputs.Select(val => (float)val.Metrics[MetricNames.Score].Value))
                .ToList(),
        };

        return state with { Runs = state.Runs };
    }
    
    private static RootState AddRun(RootState state, RunCreatedAction action)
    {
        state.Runs[action.Run.RunId] = action.Run with { Index = state.Runs.Count };
        state.UiState.RunTemplates[action.Run.RunId] = action.Run.Inputs.ModelType switch
        {
            ModelTypes.Graph => new GraphTemplate(),
            ModelTypes.Image => new ImageTemplate(),
            ModelTypes.Tree => new TreeTemplate(),
            ModelTypes.MultiObjective => new MultiObjectiveTemplate()
        };
        return state with { Runs = state.Runs, UiState = state.UiState };
    }
    
    private static RootState RunCompleted(RootState state, EngineStoppedAction action)
    {
        state.Runs[action.RunId] = state.Runs[action.RunId] with
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
        state.UiState.EngineStateExpanded[action.RunId] = action.Expanded;
        return state with { UiState = state.UiState };
    }
    
    private static RootState LayoutChanged(RootState state, LayoutChangedAction action) => state with
    {
        UiState = state.UiState with { IsSidebarOpen = action.IsSidebarOpen }
    };

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
        var run = state.Runs[action.RunId];
        state.Runs[action.RunId] = run with
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
    
    private static RootState CopyRun(RootState state, CopyRunAction action)
    {
        var (copyId, newId) = action;
        
        var copyRun = state.Runs[copyId];
        var newRuns = state.Runs.ToDictionary(pair => pair.Key, pair => pair.Value);
        newRuns[newId] = new RunState
        {
            RunId = newId,
            Index = state.Runs.Count,
            Inputs = copyRun.Inputs with { },
        };

        state.UiState.RunTemplates[newId] = state.Runs[copyId].Inputs.ModelType switch
        {
            ModelTypes.Graph => new GraphTemplate(),
            ModelTypes.Image => new ImageTemplate(),
            ModelTypes.Tree => new TreeTemplate(),
            ModelTypes.MultiObjective => new MultiObjectiveTemplate()
        };
        
        return state with { Runs = newRuns, UiState = state.UiState };
    }
}