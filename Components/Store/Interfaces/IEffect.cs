namespace Radiate.Client.Components.Store.Interfaces;

public interface IEffect<in TState, in TAction>
    where TState : IState<TState>
    where TAction : IStateAction
{
    Task Handle(TState state, TAction action);
}
