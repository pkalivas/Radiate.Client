using Radiate.Client.Services.Store.Actions;
using Radiate.Engines.Schema;
using Reflow.Reducers;

namespace Radiate.Client.Services.Store;

public static class RootReducer
{
    public static IEnumerable<On<RootState>> CreateReducers() => new List<On<RootState>>
    {
        Reducer.On<NavigateToRunAction, RootState>((state, action) => state with { CurrentRunId = action.RunId }),
        Reducer.On<RunCreatedAction, RootState>(AddRun),
        Reducer.On<AddEngineOutputAction, RootState>(AddOutput),
        Reducer.On<EngineStoppedAction, RootState>(RunCompleted),
        Reducer.On<StartEngineAction, RootState>(StartEngine),
        Reducer.On<PauseEngineRunAction, RootState>(PauseEngine),
        Reducer.On<ResumeEngineRunAction, RootState>(ResumeEngine),
        Reducer.On<SetEngineTreeExpandedAction, RootState>(SetTreeExpansions),
        Reducer.On<LayoutChangedAction, RootState>(LayoutChanged),
        Reducer.On<UpdateCurrentImageAction, RootState>(UpdateCurrentImage),
    };
    
    private static RootState AddOutput(RootState state, AddEngineOutputAction action)
    {
        var engineOutputsGeneratedAction = action.EngineOutputs;
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with
        {
            Outputs = engineOutputsGeneratedAction,
            Scores = state.Runs[state.CurrentRunId].Scores
                .Concat(new[]
                {
                    engineOutputsGeneratedAction.Metrics.Get(MetricNames.Score).Statistics.LastValue
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
            IsCompleted = true
        };
        return state with { Runs = state.Runs };
    }
    
    private static RootState StartEngine(RootState state, StartEngineAction action)
    {
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with
        {
            IsRunning = true,
            IsPaused = false,
            IsCompleted = false
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
    
    private static RootState SetTreeExpansions(RootState state, SetEngineTreeExpandedAction action)
    {
        state.UiFeature.EngineStateExpanded[action.RunId] = action.Expanded;
        return state with { UiFeature = state.UiFeature };
    }
    
    private static RootState LayoutChanged(RootState state, LayoutChangedAction action) => state with
    {
        UiFeature = state.UiFeature with { IsSidebarOpen = action.IsSidebarOpen }
    };

    private static RootState UpdateCurrentImage(RootState state, UpdateCurrentImageAction action)
    {
	    state.Images[action.RunId] = state.Images[action.RunId] with { Current = action.Image };
	    return state with { Images = state.Images };
    }
}