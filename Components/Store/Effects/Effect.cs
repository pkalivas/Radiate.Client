using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public abstract class Effect<TState, TAction> : IEffect<TState, TAction>
    where TState : class
    where TAction : IAction
{
    protected readonly IServiceProvider ServiceProvider;

    protected Effect(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public abstract Task HandleAsync(TState state, TAction action, IDispatcher dispatcher);

    public bool CanHandle(IState feature, IAction action) => feature is IState<TState> && action is TAction;

    public Task HandleAsync(IState feature, IAction action, IDispatcher dispatcher)
    {
        if (feature is IState<TState> tState && action is TAction tAction)
        {
            return HandleAsync(tState.GetValue(), tAction, dispatcher);
        }

        return Task.CompletedTask;
    }
}