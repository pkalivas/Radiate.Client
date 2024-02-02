using Radiate.Client.Components.Store.Interfaces;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store;

public class State<T> : IState<T> where T : ICopy<T>
{
    private T Value { get; set; }
    private readonly ThrottledInvoker _throttledInvoker;
    
    public State(T value)
    {
        Value = value;
        _throttledInvoker = new ThrottledInvoker(ThrottledInvoke);
    }
    
    public T GetValue() => Value;
    public IState GetState() => this;
    public string Name => Value.GetType().Name;
    public void SetState(IState state) => SetState((T)state);

    public int ChangeCount { get; private set; }
    
    public event EventHandler<T>? SelectedValueChanged;
    public event EventHandler? StateChanged;

    public void Reduce(IReducer reducer, IAction action)
    {
        if (reducer is IReducer<T> tReducer)
        {
            SetState(tReducer.Reduce(Value.Copy(), action));
        }
    }

    public IState<TK> SelectState<TK>(Func<T, TK> selector)  where TK : ICopy<TK>
    {
        var newState = new StateSelection<T, TK>(this, selector);

        return newState;
    }

    protected void SetState(T state)
    {
        if (Equals(state, Value))
        {
            return;
        }
        
        Value = state;
        ChangeCount++;
        _throttledInvoker.Invoke(40);
    }
    
    private void ThrottledInvoke()
    {
        SelectedValueChanged?.Invoke(this, Value);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}

