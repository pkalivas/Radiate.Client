using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store.Reducers;

public static class RunUiReducers
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
    [
        Reducer.On<SetEngineTreeExpandedAction, RootState>(SetTreeExpansions),
        Reducer.On<RunUiCreatedAction, RootState>(AddRunUi),
    ];

    private static RootState AddRunUi(RootState state, RunUiCreatedAction action) => state
        .UpdateRunUi(action.RunUi.RunId, _ => action.RunUi);
    
    private static RootState SetTreeExpansions(RootState state, SetEngineTreeExpandedAction action) => state
        .UpdateRunUi(action.RunId, ui => ui with
        {
            EngineStateExpanded = action.Expanded.ToImmutableDictionary()
        });
}