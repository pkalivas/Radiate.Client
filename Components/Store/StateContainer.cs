using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateContainer : IStateContainer
{
    private IState _state = default!;
    
    public event Action? OnChange;
    
    public StateContainer(IState state)
    {
        _state = state;
    }
    
    public void NotifyStateChanged() => OnChange?.Invoke();
    
    public void SetState(IState state)
    {
        _state = state;
    }

    public TState GetState<TState>() where TState : IState<TState> => (TState) _state;
    
    public IState GetState(Type stateType) => _state;
}
