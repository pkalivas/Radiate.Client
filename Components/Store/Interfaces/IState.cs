namespace Radiate.Client.Components.Store.Interfaces;

public interface IState
{
    event Action OnChange;
    void NotifyStateChanged();
}

public interface IState<TState> : IState
    where TState : IState<TState>, IState
{
}
