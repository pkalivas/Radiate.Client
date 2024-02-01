namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore : IDisposable
{
    Task Dispatch<TAction, TState>(TAction action)
        where TAction : IAction<TState, TAction>
        where TState : IState<TState>;
    
    TState GetFeature<TState>() where TState : IState<TState>;
    StateContainer GetStateContainer<TState>() where TState : IState<TState>;
    void Register<TState>(TState state) where TState : IState<TState>;
}