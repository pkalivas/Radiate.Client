namespace Radiate.Client.Components.Store.Interfaces;

public interface IState : IStateChangeNotifier
{
    string Name { get; }
    IState GetState();
}

public interface IState<out TState> : IState
    where TState : IState<TState>
{
}