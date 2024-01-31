using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.Actions;

public abstract record StateAction<TState> : IAction<TState> where TState : IState
{
    public string StateName => typeof(TState).Name;
    public abstract Task Reduce(TState feature);
}