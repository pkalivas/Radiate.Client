using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Actions;

public abstract record StateAction<TState, TAction> : IAction<TState, TAction>
    where TState : IState<TState>
    where TAction : IAction
{
    public string StateName => typeof(TState).Name;
    public abstract Task Reduce(TState feature);
}

