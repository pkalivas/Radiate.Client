using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Interfaces;

public interface IAction { }

public interface IAction<in TState, TAction> : IAction
    where TState : IState<TState>
    where TAction : IAction 
{
    string StateName { get; }
    Task Reduce(TState feature);
}

public record CountAction : StateAction<AppState, CountAction>
{
    public override Task Reduce(AppState feature)
    {
        throw new NotImplementedException();
    }

    public int Amount { get; init; }
}