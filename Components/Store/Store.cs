using System.Collections.Concurrent;
using Radiate.Client.Components.Store.Interfaces;

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
    private readonly Dictionary<string, List<IEffect>> _effects = new();
    private readonly Dictionary<string, StateContainer> _stateContainers = new();
    private readonly ConcurrentQueue<IAction> _actionQueue = new();

    public StateStore(IDispatcher dispatcher, IEnumerable<IEffect> effects, IEnumerable<IReducer> reducers)
    {
        _dispatcher = dispatcher;
        _dispatcher.OnDispatch += ActionDispatched;
        SetEffects(effects);
        SetReducers(reducers);
    }
    
    public bool IsDispatching { get; private set; }
    
    public async Task Dispatch<TAction, TState>(TAction action) 
        where TAction : IAction<TState> 
        where TState : IState<TState>
    {
        throw new NotImplementedException();
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
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IAction<>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();
            
            if (stateType == null)
            {
                throw new InvalidOperationException($"Action {actionType.Name} does not implement IAction<TState>");
            }
            
            var stateContainer = _stateContainers[stateType.Name];
            var state = stateContainer.GetState(stateType);
            var actionReducers = _reducers[stateType.Name];
            
            var tasks = new List<Task>();
            foreach (var reducer in actionReducers)
            {
                state = reducer.Reduce(state, action);
                stateContainer.SetState(state!);
                stateContainer.NotifyStateChanged();
                
                if (_effects.TryGetValue(actionType.Name, out var effects))
                {
                    foreach (var effect in effects.Where(effect => effect.CanHandle(state, action)))
                    {
                        tasks.Add(Task.Run(async () => await effect.HandleAsync(state, action, _dispatcher)));
                    }
                }
            }
        }
    }
    
    private void SetEffects(IEnumerable<IEffect> effects)
    {
        foreach (var effect in effects)
        {
            var effectType = effect.GetType();
            var actionType = effectType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEffect<,>))
                .Select(i => i.GetGenericArguments()[1])
                .FirstOrDefault();
            
            if (actionType == null)
            {
                throw new InvalidOperationException($"Effect {effectType.Name} does not implement IEffect<TState, TAction>");
            }
            
            if (!_effects.TryGetValue(actionType.Name, out var actionEffects))
            {
                actionEffects = new List<IEffect>();
                _effects.Add(actionType.Name, actionEffects);
            }
            
            actionEffects.Add(effect);
        }
    }
    
    private void SetReducers(IEnumerable<IReducer> reducers)
    {
        foreach (var reducer in reducers)
        {
            var reducerType = reducer.GetType();
            var actionType = reducerType.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IReducer<>))
                .Select(i => i.GetGenericArguments()[0])
                .FirstOrDefault();
            
            if (actionType == null)
            {
                throw new InvalidOperationException($"Reducer {reducerType.Name} does not implement IReducer<TState>");
            }
            
            if (!_reducers.TryGetValue(actionType.Name, out var actionReducers))
            {
                actionReducers = new List<IReducer>();
                _reducers.Add(actionType.Name, actionReducers);
            }
            
            actionReducers.Add(reducer);
        }
    }


    public StateContainer GetStateContainer<TState>() where TState : IState<TState> =>
        _stateContainers[typeof(TState).Name];

    public void Register<TState>(TState state) 
        where TState : IState<TState>
    {
        _stateContainers.Add(typeof(TState).Name, new StateContainer(state));
    }

    public bool IsRegistered<TState>() where TState : IState<TState> =>
        _stateContainers.ContainsKey(typeof(TState).Name);

    public TState GetFeature<TState>() where TState : IState<TState> =>
        _stateContainers[typeof(TState).Name].GetState<TState>();

    public void Dispose()
    {
        _dispatcher.OnDispatch -= ActionDispatched;
    }
}