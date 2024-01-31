using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class EventContainer
{
    public event Action OnChange;
    
    public void NotifyStateChanged() => OnChange?.Invoke();
}

public class StateStore : IStore
{
    private readonly Dictionary<string, IState> _states = new();
    private readonly Dictionary<string, EventContainer> _actions = new();
    
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

    public EventContainer GetAction<TState>() where TState : IState<TState>
    {
        var stateName = typeof(TState).Name;
        if (_actions.TryGetValue(stateName, out var action))
        {
            return action;
        }
        
        _actions.Add(stateName, new EventContainer());
        return _actions[stateName];
    }

    public TState GetFeature<TState>() where TState : IState<TState> =>
        (TState)_states[typeof(TState).Name];
}