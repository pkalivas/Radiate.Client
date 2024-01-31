using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.States;

public abstract record StoreState<TState> : IState<TState> where TState : IState<TState>
{
    public abstract Guid Id { get; }
    public event Action OnChange;
    public void NotifyStateChanged() => OnChange?.Invoke();
}
