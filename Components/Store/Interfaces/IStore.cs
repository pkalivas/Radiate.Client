namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore
{
    Task Dispatch<TAction, TState>(TAction action)
        where TAction : IAction<TState>
        where TState : IState<TState>;
    
    TState GetFeature<TState>() where TState : IState<TState>;
    void Register<TState>(TState state) where TState : IState<TState>;
    EventContainer GetAction<TState>() where TState : IState<TState>;   
}