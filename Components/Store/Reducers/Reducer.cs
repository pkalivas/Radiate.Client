using Radiate.Client.Components.Store.Interfaces;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.Reducers;

public abstract class Reducer<TState> : IReducer<TState>, IReducer
    where TState : class, ICopy<TState>
{
    public abstract TState Reduce(TState state, IAction action);

    public IState Reduce(IState feature, IAction action)
    {
        if (feature is IState<TState> tState)
        {
            return Reduce(tState.GetValue(), action) as IState<TState>;
        }

        throw new InvalidOperationException("Invalid feature type");
    }
}