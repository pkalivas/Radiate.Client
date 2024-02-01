using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public abstract class RootEffect<TState, TAction> : IEffect<TState, TAction>
    where TState : class, IState<TState>
    where TAction : IAction
{
    protected readonly IServiceProvider ServiceProvider;

    protected RootEffect(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public abstract Task HandleAsync(TState state, TAction action, IDispatcher dispatcher);

    public bool CanHandle(IState feature, IAction action) => feature is TState && action is TAction;

    public Task HandleAsync(IState feature, IAction action, IDispatcher dispatcher)
    {
        if (feature is TState tState && action is TAction tAction)
        {
            return HandleAsync(tState, tAction, dispatcher);
        }

        return Task.CompletedTask;
    }
}