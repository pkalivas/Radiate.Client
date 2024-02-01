using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Reducers;

public class AppStateReducer : RootReducer<AppState>
{
    public override AppState Reduce(AppState state, IAction action) => action switch
    {
        CountAction countAction => state with { Count = state.Count + 1 },
        _ => state
    };
}