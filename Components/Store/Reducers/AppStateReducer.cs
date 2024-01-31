using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Reducers;

public class AppStateReducer : IReducer<AppState>
{
    public AppState Reduce(AppState state, IStateAction action) =>
        action switch
        {
            CountAction appStateAction => state with { Count = state.Count += 1 },
            _ => throw new NotImplementedException()
        };
}