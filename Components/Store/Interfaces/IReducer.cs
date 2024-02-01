namespace Radiate.Client.Components.Store.Interfaces;

public interface IReducer
{
    IState Reduce(IState feature, IAction action);
}

public interface IReducer<TState>
{
    TState Reduce(TState state, IAction action);
}