namespace Radiate.Client.Components.Store.Interfaces;

public interface IReducer<TState> where TState : IState<TState>
{
    TState Reduce(TState state, IStateAction action);
}