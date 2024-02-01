using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Reducers;

public abstract class RootReducer<TState> : IReducer<TState>, IReducer
    where TState : class
{
    public abstract TState Reduce(TState state, IAction action);

    public IState Reduce(IState feature, IAction action)
    {
        if (feature is IState<TState> tState)
        {
            return (IState) Reduce(tState.GetValue(), action);
        }

        throw new InvalidOperationException("Invalid feature type");
    }
}