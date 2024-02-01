using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public interface IDispatcher
{
    event EventHandler<IAction> OnDispatch;
    void Dispatch<TAction, TState>(TAction action) 
        where TAction : IAction
        where TState : IFeature<TState>;   
}

public class Dispatcher : IDispatcher
{
    private readonly object _syncRoot = new();
    private readonly Queue<IAction> _actionQueue = new();
    private EventHandler<IAction> _onDispatch;
    
    public event EventHandler<IAction> OnDispatch
    {
        add
        {
            lock (_syncRoot)
            {
                _onDispatch += value;
                while (_actionQueue.Count > 0)
                {
                    value(this, _actionQueue.Dequeue());
                }
            }
        }
        remove
        {
            lock (_syncRoot)
            {
                _onDispatch -= value;
            }
        }
    }
    
    public void Dispatch<TAction, TState>(TAction action) 
        where TAction : IAction
        where TState : IFeature<TState>
    {
        lock (_syncRoot)
        {
            _actionQueue.Enqueue(action);
            _onDispatch?.Invoke(this, action);
        }
    }
}