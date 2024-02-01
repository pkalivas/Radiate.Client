using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Reducers;

public abstract class RootReducer<TState> : IReducer<TState>, IReducer
    where TState : class, IFeature
{
    public abstract TState Reduce(TState state, IAction action);

    public IFeature Reduce(IFeature feature, IAction action)
    {
        if (feature is TState tState)
        {
            return Reduce(tState, action);
        }

        return feature;
    }
}