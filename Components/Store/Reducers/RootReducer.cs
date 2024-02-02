using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Engines.Schema;

namespace Radiate.Client.Components.Store.Reducers;

public class RootReducer : Reducer<RootFeature>
{
    public override RootFeature Reduce(RootFeature state, IAction action) => action switch
    {
        NavigateToRunAction navigateToRunAction => state with { CurrentRunId = navigateToRunAction.RunId },
        RunCreatedAction runCreatedAction => AddRun(state, runCreatedAction),
        AddEngineOutputAction engineOutputsGeneratedAction => AddOutput(state, engineOutputsGeneratedAction),
        RunCompletedAction runCompletedAction => RunCompleted(state, runCompletedAction),
        StartEngineAction startEngineAction => StartEngine(state, startEngineAction),
        SetEngineTreeExpandedAction setTreeExpansionsAction => SetTreeExpansions(state, setTreeExpansionsAction),
        LayoutChangedAction layoutChangedAction => LayoutChanged(state, layoutChangedAction),
        UpdateCurrentImageAction updateImageAction => 
        _ => state
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

    private static RootFeature UpdateCurrentImage(RootFeature state, UpdateCurrentImageAction action) => state with
    {
    };
}