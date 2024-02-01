using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public abstract class RootEffect<TState, TAction> : IEffect<TState, TAction>
    where TState : class, IFeature<TState>
    where TAction : IAction
{
    protected readonly IServiceProvider ServiceProvider;

    protected RootEffect(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

    public abstract Task HandleAsync(TState state, TAction action, IDispatcher dispatcher);

    public bool CanHandle(IFeature feature, IAction action) => feature is TState && action is TAction;

    public Task HandleAsync(IFeature feature, IAction action, IDispatcher dispatcher)
    {
        if (feature is TState tState && action is TAction tAction)
        {
            return HandleAsync(tState, tAction, dispatcher);
        }

        return Task.CompletedTask;
    }
}