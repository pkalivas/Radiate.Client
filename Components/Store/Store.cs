using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;


// public interface IEffect 
// {
//     Task Execute<TState, TAction>(TState state, TAction action, IStore store)
//         where TState : IState
//         where TAction : IAction<TState>;
// }

public class StateStore : IStore
{
    private readonly Dictionary<string, IState> _states = new();
    
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

    public void Subscribe<TState, TAction>(string subscriber, Func<TState, TAction, Task> callback) 
        where TState : IState<TState>
        where TAction : IAction<TState>
    {
        var key = $"{typeof(TState).Name}-{typeof(TAction).Name}";
        // if (!_subscriptions.ContainsKey(key))
        // {
        //     _subscriptions.Add(key, new List<Task>());
        // }
        //
        // if (_subscriptions.TryGetValue(key, out var subscriptions))
        // {
        //     
        // }
    }

    public TState GetFeature<TState>() where TState : IState<TState> =>
        (TState)_states[typeof(TState).Name];
}