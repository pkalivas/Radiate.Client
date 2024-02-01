using System.Collections.Concurrent;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store;

public class State<TState> : StateSelection<TState, TState>
    where TState : IState<TState>
{
    public State(IFeature<TState> feature) : base(feature, (TState) feature.GetState(), val => val)
    {
    }
    
    public override int GetHashCode() => State.GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is State<TState> state)
        {
            return ReferenceEquals(state, State);
        }

        return false;
    }
}

public class StateStore : IStore
{
    private readonly object _syncRoot = new();
    private readonly IDispatcher _dispatcher;
    private readonly IActionSubscriber _actionSubscriber;
    private readonly Dictionary<string, List<IReducer>> _reducers = new();
    private readonly Dictionary<string, List<IEffect>> _effects = new();
    private readonly Dictionary<string, IFeature> _features = new();
    private readonly Dictionary<string, StateContainer> _stateContainers = new();
    private readonly ConcurrentQueue<IAction> _actionQueue = new();

    public StateStore(IDispatcher dispatcher, IEnumerable<IEffect> effects, IEnumerable<IReducer> reducers)
    {
        _dispatcher = dispatcher;
        _dispatcher.OnDispatch += ActionDispatched!;
        _actionSubscriber = new ActionSubscriber();
        SetEffects(effects);
        SetReducers(reducers);
    }

    private bool IsDispatching { get; set; }
    
    public async Task Dispatch<TAction, TState>(TAction action) 
        where TAction : IAction<TState> 
        where TState : IFeature<TState>, IState<TState>
    {
        throw new NotImplementedException();
    }

    public State<TState> Select<TState>() where TState : IState<TState> 
    {
        var feature = (Feature2<TState>)_features[typeof(TState).Name];
        var newState = new State<TState>(feature);
        
        feature.SelectedValueChanged += (sender, state) => newState.OnStateChanged(state);

        return newState;
    }
    
    public void Register<TState>(TState state) 
        where TState : IState<TState>
    {
        _features[typeof(TState).Name] = new Feature2<TState>(state);
    }
    
    public void Notify(IAction action) => _actionSubscriber.Notify(action);
    
    public void Subscribe<TAction>(object subscriber, Action<TAction> callback) where TAction : IAction =>
        _actionSubscriber.Subscribe(subscriber, callback);

    public void Unsubscribe<TAction>(object subscriber) where TAction : IAction =>
        _actionSubscriber.Unsubscribe<TAction>(subscriber);

    public void UnsubscribeAll(object subscriber) => _actionSubscriber.UnsubscribeAll(subscriber);

    public void Dispose() =>  _dispatcher.OnDispatch -= ActionDispatched!;
    
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
            
            var feature = _features[stateType.Name];
            var reducers = _reducers[stateType.Name];
            var currentState = feature.GetState();
            
            var tasks = new List<Task>();
            foreach (var reducer in reducers)
            {
                currentState = reducer.Reduce(currentState, action);
                feature.SetState(currentState);
                // stateContainer.SetState(state);
                // stateContainer.NotifyStateChanged();
                
                if (_effects.TryGetValue(actionType.Name, out var effects))
                {
                    tasks.AddRange(effects
                        .Where(effect => effect.CanHandle(currentState, action))
                        .Select(effect => effect.HandleAsync(currentState, action, _dispatcher)));
                }
            }
            
            _actionSubscriber.Notify(action);
            
            Task.Run(async () => await Task.WhenAll(tasks));
            
            
            //
            // var stateContainer = _stateContainers[stateType.Name];
            // var state = stateContainer.GetState(stateType);
            // var actionReducers = _reducers[stateType.Name];
            
            // var tasks = new List<Task>();
            // foreach (var reducer in actionReducers)
            // {
            //     state = reducer.Reduce(state, action);
            //     stateContainer.SetState(state);
            //     stateContainer.NotifyStateChanged();
            //     
            //     if (_effects.TryGetValue(actionType.Name, out var effects))
            //     {
            //         tasks.AddRange(effects
            //             .Where(effect => effect.CanHandle(state, action))
            //             .Select(effect => effect.HandleAsync(state, action, _dispatcher)));
            //     }
            // }
            //
            // _actionSubscriber.Notify(action);
            //
            // Task.Run(async () => await Task.WhenAll(tasks));
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
}