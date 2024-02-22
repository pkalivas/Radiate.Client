using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store.Reducers;

public static class UiReducers
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
    [
        Reducer.On<NavigateToRunAction, RootState>(NavigateToRun),
        Reducer.On<LayoutChangedAction, RootState>(LayoutChanged),
        Reducer.On<SetRunLoadingAction, RootState>(SetRunLoading)
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
    
    private static RootState SetRunLoading(RootState state, SetRunLoadingAction action) => state with
    {
        UiState = state.UiState with
        {
            LoadingStates = state.UiState.LoadingStates.ContainsKey(action.RunId)
                ? state.UiState.LoadingStates.SetItem(action.RunId, action.Loading).ToImmutableDictionary()
                : state.UiState.LoadingStates.Add(action.RunId, action.Loading).ToImmutableDictionary()
        }
    };
}