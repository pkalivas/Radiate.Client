using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.Reducers;

namespace Radiate.Client.Components.Store.States;

public abstract record StoreState<TState> : IState<TState> where TState : IState<TState>
{
    public List<IReducer<TState>> Reducers { get; init; } = new();
}
