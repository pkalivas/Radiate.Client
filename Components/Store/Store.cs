using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store;


public interface IStateActionHandler<in TState, in TAction>
    where TState : IState<TState>
    where TAction : IAction<TState>
{
    Task Handle(TState state, TAction action, IStore store);
}

public class TestHandler : IStateActionHandler<AppState, FunctionalAction>
{
    public Task Handle(AppState state, FunctionalAction action, IStore store)
    {
        throw new NotImplementedException();
    }
}

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

    public TState GetFeature<TState>() where TState : IState<TState> =>
        (TState)_states[typeof(TState).Name];
}