using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public abstract class RootEffect<TState, TAction> : IEffect<TState, TAction>
    where TState : class, IState<TState>
    where TAction : IAction
{
    public abstract Task HandleAsync(TState state, TAction action, IDispatcher dispatcher);

    public bool CanHandle(IState state, IAction action) => state is TState && action is TAction;

    public Task HandleAsync(IState state, IAction action, IDispatcher dispatcher)
    {
        if (state is TState tState && action is TAction tAction)
        {
            return HandleAsync(tState, tAction, dispatcher);
        }

        return Task.CompletedTask;
    }
}