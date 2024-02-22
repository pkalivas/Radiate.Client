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
        Reducer.On<SetRunTemplateAction, RootState>(SetRunTemplate),
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
    
    private static RootState SetRunTemplate(RootState state, SetRunTemplateAction action)
    {

        return state;
    }
        
        // => state
        // .UpdateUi(ui => ui with
        // {
        //     RunTemplates = ui.RunTemplates.ContainsKey(action.RunId)
        //         ? ui.RunTemplates
        //             .ToImmutableDictionary(pair => pair.Key, pair => pair.Key == action.RunId 
        //                 ? action.Template 
        //                 : pair.Value)
        //         : ui.RunTemplates
        //             .Concat([new KeyValuePair<Guid, IRunTemplate>(action.RunId, action.Template)])
        //             .ToImmutableDictionary(pair => pair.Key, pair => pair.Value)
        // });
}