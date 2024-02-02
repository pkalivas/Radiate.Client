using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class Subscription
{
    public Type? SubsciberType { get; set; }
    public Type? ActionType { get; set; }
    public Action<IAction> Callback { get; set; }
}

public class ActionSubscriber : IActionSubscriber
{
    private readonly object _lock = new();
    private readonly Dictionary<Type, List<Subscription>> _subscriptions = new();
    
    public void Subscribe<TAction>(object subscriber, Action<TAction> callback) where TAction : IAction
    {
        var actionType = typeof(TAction);
        var subscriberType = subscriber.GetType();
        
        if (!_subscriptions.ContainsKey(actionType))
        {
            _subscriptions.Add(actionType, new List<Subscription>());
        }
        
        _subscriptions[actionType].Add(new Subscription
        {
            SubsciberType = subscriberType,
            ActionType = actionType,
            Callback = action => callback((TAction) action)
        });
    }

    public void Unsubscribe<TAction>(object subscriber) where TAction : IAction
    {
        var actionType = typeof(TAction);
        var subscriberType = subscriber.GetType();
        
        if (!_subscriptions.ContainsKey(actionType))
        {
            return;
        }
        
        _subscriptions[actionType].RemoveAll(subscription => subscription.SubsciberType == subscriberType);
    }

    public void UnsubscribeAll(object subscriber)
    {
        lock (_lock)
        {
            var subscriberType = subscriber.GetType();
            
            foreach (var key in _subscriptions.Keys)
            {
                _subscriptions[key].RemoveAll(subscription => subscription.SubsciberType == subscriberType);
            }
        }
    }
    
    public void Notify(IAction action)
    {
        var actionType = action.GetType();
        
        if (!_subscriptions.ContainsKey(actionType))
        {
            if (_subscriptions.TryGetValue(typeof(IAction), out var subscriptions))
            {
                foreach (var subscription in subscriptions)
                {
                    subscription.Callback(action);
                }
            }
        }
        else
        {
            foreach (var subscription in _subscriptions[actionType])
            {
                subscription.Callback(action);
            }
        }
    }
}