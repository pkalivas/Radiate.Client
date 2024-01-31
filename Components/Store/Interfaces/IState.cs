namespace Radiate.Client.Components.Store.Interfaces;

public interface IState
{
    event Action OnChange;
    void NotifyStateChanged();
}
