namespace Radiate.Client.Components.Store.Interfaces;

public interface IEffect
{
    bool CanHandle(IState feature, IAction action);
    Task HandleAsync(IState feature, IAction action, IDispatcher dispatcher);
}

public interface IEffect<in TState, in TAction> : IEffect
{
    Task HandleAsync(TState state, TAction action, IDispatcher dispatcher);
}
