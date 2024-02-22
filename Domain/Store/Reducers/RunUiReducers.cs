using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store.Reducers;

public static class RunUiReducers
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
    [
        Reducer.On<SetEngineTreeExpandedAction, RootState>(SetTreeExpansions),
    ];
    
    private static RootState SetTreeExpansions(RootState state, SetEngineTreeExpandedAction action) => state
        .UpdateRunUi(action.RunId, ui => ui with
        {
            EngineStateExpanded = action.Expanded.ToImmutableDictionary()
        });
}