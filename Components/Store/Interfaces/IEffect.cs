namespace Radiate.Client.Components.Store.Interfaces;

public interface IEffect
{
    bool CanHandle(IFeature feature, IAction action);
    Task HandleAsync(IFeature feature, IAction action, IDispatcher dispatcher);
}

public interface IEffect<in TState, in TAction> : IEffect
    where TState : IFeature<TState>
{
    Task HandleAsync(TState state, TAction action, IDispatcher dispatcher);
}
