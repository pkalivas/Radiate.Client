namespace Radiate.Client.Components.Store.Interfaces;

public interface IStateContainer
{
    event Action OnChange;
    void NotifyStateChanged();
    TState GetState<TState>() where TState : IFeature<TState>;
}