using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateStore : IStore
{
    private readonly Dictionary<string, IState> _states = new();
    // private readonly Dictionary<string, List<>
    
    public async Task Dispatch<TAction, TState>(TAction action) 
        where TAction : IAction<TState> 
        where TState : IState<TState>
    {
        if (_states.TryGetValue(action.StateName, out var state))
        {
            await action.Reduce((TState)state);
            state.NotifyStateChanged();
            return;
        }
        
        throw new InvalidOperationException($"Feature {action.StateName} not found");
    }
    
    public void Register<TState>(TState state) 
        where TState : IState<TState>
    {
        _states.Add(typeof(TState).Name, state);
    }

    public TState GetFeature<TState>() where TState : IState<TState> =>
        (TState)_states[typeof(TState).Name];
}