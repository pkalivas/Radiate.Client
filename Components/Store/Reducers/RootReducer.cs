using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Engines.Schema;
using Reflow.Reducers;

namespace Radiate.Client.Components.Store.Reducers;

public static class RootReducer
{
    public static IEnumerable<On<RootFeature>> CreateReducers() => new List<On<RootFeature>>
    {
        Reducer.On<NavigateToRunAction, RootFeature>((state, action) => state with { CurrentRunId = action.RunId }),
        Reducer.On<RunCreatedAction, RootFeature>(AddRun),
        Reducer.On<AddEngineOutputAction, RootFeature>(AddOutput),
        Reducer.On<RunCompletedAction, RootFeature>(RunCompleted),
        Reducer.On<StartEngineAction, RootFeature>(StartEngine),
        Reducer.On<SetEngineTreeExpandedAction, RootFeature>(SetTreeExpansions),
        Reducer.On<LayoutChangedAction, RootFeature>(LayoutChanged),
        Reducer.On<UpdateCurrentImageAction, RootFeature>(UpdateCurrentImage),
    };
    
    private static RootFeature AddOutput(RootFeature state, AddEngineOutputAction action)
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
    
    private static RootFeature AddRun(RootFeature state, RunCreatedAction action)
    {
        state.Runs[action.Run.RunId] = action.Run with { Index = state.Runs.Count };
        return state with { Runs = state.Runs };
    }
    
    private static RootFeature RunCompleted(RootFeature state, RunCompletedAction action)
    {
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with { IsRunning = false };
        return state with { Runs = state.Runs };
    }
    
    private static RootFeature StartEngine(RootFeature state, StartEngineAction action)
    {
        state.Runs[state.CurrentRunId] = state.Runs[state.CurrentRunId] with { IsRunning = true };
        return state with { Runs = state.Runs };
    }
    
    private static RootFeature SetTreeExpansions(RootFeature state, SetEngineTreeExpandedAction action)
    {
        state.UiState.EngineStateExpanded[action.RunId] = action.Expanded;
        return state with { UiState = state.UiState };
    }
    
    private static RootFeature LayoutChanged(RootFeature state, LayoutChangedAction action) => state with
    {
        UiState = state.UiState with { IsSidebarOpen = action.IsSidebarOpen }
    };

    private static RootFeature UpdateCurrentImage(RootFeature state, UpdateCurrentImageAction action)
    {
	    state.Images[action.RunId] = state.Images[action.RunId] with { Current = action.Image };
	    return state with { Images = state.Images };
    }
}