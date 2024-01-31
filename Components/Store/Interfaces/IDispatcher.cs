namespace Radiate.Client.Components.Store.Interfaces;

public interface IDispatcher
{
    void Dispatch<TAction, TState>(TAction action)
        where TAction : IAction<TState>
        where TState : IState<TState>;
}