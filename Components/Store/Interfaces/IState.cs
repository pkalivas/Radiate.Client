namespace Radiate.Client.Components.Store.Interfaces;

public interface IState { }

public interface IState<TState> : IState
    where TState : IState<TState>, IState
{
    
}
