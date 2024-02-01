namespace Radiate.Client.Components.Store.Interfaces;

public interface IAction { }

public interface IAction<in TState> : IAction
    where TState : IState<TState> { }
    