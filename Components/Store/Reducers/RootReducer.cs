using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Reducers;

public abstract class RootReducer<TState> : IReducer<TState>, IReducer
    where TState : class, IState
{
    public abstract TState Reduce(TState state, IAction action);

    public IState Reduce(IState state, IAction action)
    {
        if (state is TState tState)
        {
            return Reduce(tState, action);
        }

        return state;
    }
}