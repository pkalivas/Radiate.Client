namespace Radiate.Client.Components.Store.Interfaces;

public interface IState : IStateChangeNotifier
{
}

public interface IState<out TState> : IState
    where TState : IState<TState>
{
    TState State { get; }
}