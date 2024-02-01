using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateContainer : IStateContainer
{
    private IFeature _feature = default!;
    
    public event Action? OnChange;
    
    public StateContainer(IFeature feature)
    {
        _feature = feature;
    }
    
    public void NotifyStateChanged() => OnChange?.Invoke();
    
    public void SetState(IFeature feature)
    {
        _feature = feature;
    }

    public TState GetState<TState>() where TState : IFeature<TState> => (TState) _feature;
    
    public IFeature GetState(Type stateType) => _feature;
}
