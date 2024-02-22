using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store.Reducers;

public static class UiReducers
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
    [
        Reducer.On<NavigateToRunAction, RootState>(NavigateToRun),
        Reducer.On<SetEngineTreeExpandedAction, RootState>(SetTreeExpansions),
        Reducer.On<LayoutChangedAction, RootState>(LayoutChanged),
    ];
    
    private static RootState NavigateToRun(RootState state, NavigateToRunAction action) => state with
    {
        CurrentRunId = action.RunId,
        UiState = state.UiState with
        {
            IsSidebarOpen = false
        }
    };
    
    private static RootState LayoutChanged(RootState state, LayoutChangedAction action) => state with
    {
        UiState = state.UiState with { IsSidebarOpen = action.IsSidebarOpen }
    };
    
    private static RootState SetTreeExpansions(RootState state, SetEngineTreeExpandedAction action) => state
        .UpdateUi(ui => ui with
        {
            EngineStateExpanded = ui.EngineStateExpanded.ContainsKey(action.RunId)
                ? ui.EngineStateExpanded
                    .ToImmutableDictionary(pair => pair.Key, pair => pair.Key == action.RunId ? action.Expanded : pair.Value)
                : ui.EngineStateExpanded
                    .Concat([new KeyValuePair<Guid, Dictionary<string, bool>>(action.RunId, action.Expanded)])
                    .ToImmutableDictionary(pair => pair.Key, pair => pair.Value)
        });
}