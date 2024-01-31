namespace Radiate.Client.Components.Store.Interfaces;

public interface IStore
{
    Task Dispatch<TAction, TSTate>(TAction action)
        where TAction : IAction<TSTate>
        where TSTate : IState;
    
    TState GetFeature<TState>() where TState : IState;
    void Register<TState>(TState state) where TState : IState;
    void Subscribe<TState, TAction>(string subscriber, Func<TState, TAction, Task> callback) 
        where TState : IState
        where TAction : IAction<TState>;
}