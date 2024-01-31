using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.States;

public abstract record State : IState
{
    public event Action OnChange;
    public void NotifyStateChanged() => OnChange?.Invoke();
}
