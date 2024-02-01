using System.Collections.Concurrent;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.Reducers;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store;

public interface IStateContainer
{
    event Action OnChange;
    void NotifyStateChanged();
    TState GetState<TState>() where TState : IState<TState>;
}

public class StateContainer : IStateContainer
{
    private IState _state = default!;
    public event Action? OnChange;
    
    public StateContainer(IState state)
    {
        _state = state;
    }
    
    public void NotifyStateChanged() => OnChange?.Invoke();
    
    public void SetState<TState>(TState state) where TState : IState<TState>
    {
        _state = state;
    }
    
    public void SetState(IState state)
    {
        _state = state;
    }

    public TState GetState<TState>() where TState : IState<TState> => (TState) _state;
    
    public IState GetState(Type stateType) => _state;
}


public class StateStore : IStore
{
    private readonly object _syncRoot = new();
    private readonly IDispatcher _dispatcher;
    private readonly Dictionary<string, List<IReducer>> _reducers = new();
    private readonly Dictionary<string, StateContainer> _stateContainers = new();
    private readonly ConcurrentQueue<IAction> _actionQueue = new();

    public StateStore(IDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
        _dispatcher.OnDispatch += ActionDispatched;
    }
    
    public bool IsDispatching { get; private set; }
    
    public async Task Dispatch<TAction, TState>(TAction action) 
        where TAction : IAction<TState, TAction> 
        where TState : IState<TState>
    {
        if (_stateContainers.TryGetValue(action.StateName, out var container))
        {
            var state = container.GetState<TState>();
            await action.Reduce(state);
            container.SetState(state);
            return;
        }
        
        throw new InvalidOperationException($"Feature {action.StateName} not found");
    }
    
    private void ActionDispatched(object sender, IAction action)
    {
        _actionQueue.Enqueue(action);
        
        if (IsDispatching)
        {
            return;
        }
        
        IsDispatching = true;
        try
        {
            lock (_syncRoot)
            {
                ProcessActionQueue();
            }
        }
        finally
        {
            IsDispatching = false;
        }
    }
    
    private void ProcessActionQueue()
    {
        while (_actionQueue.TryDequeue(out var action))
        {
            var actionType = action.GetType();
            var stateType = actionType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAction<,>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();
            
            if (stateType == null)
            {
                throw new InvalidOperationException($"Action {actionType.Name} does not implement IAction<TState>");
            }
            
            var stateContainer = _stateContainers[stateType.Name];
            var state = stateContainer.GetState(stateType);
            
            if (!_reducers.TryGetValue(actionType.Name, out var actionReducers))
            {
                actionReducers = new List<IReducer>();
                var reducerType = typeof(IReducer<>).MakeGenericType(stateType);
                var reducers = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(a => a.GetTypes())
                    .Where(t => t.GetInterfaces().Contains(reducerType));
                
                foreach (var reducer in reducers)
                {
                    actionReducers.Add((IReducer) Activator.CreateInstance(reducer)!);
                }
                
                _reducers.Add(actionType.Name, actionReducers);
            }
            
            foreach (var reducer in actionReducers)
            {
                var newState = reducer.Reduce(state, action);
                stateContainer.SetState(newState!);
            }
            
            stateContainer.NotifyStateChanged();
        }
    }

    public StateContainer GetStateContainer<TState>() where TState : IState<TState> =>
        _stateContainers[typeof(TState).Name];

    public void Register<TState>(TState state) 
        where TState : IState<TState>
    {
        _stateContainers.Add(typeof(TState).Name, new StateContainer(state));
    }

    public TState GetFeature<TState>() where TState : IState<TState> =>
        _stateContainers[typeof(TState).Name].GetState<TState>();

    public void Dispose()
    {
        _dispatcher.OnDispatch -= ActionDispatched;
    }
}