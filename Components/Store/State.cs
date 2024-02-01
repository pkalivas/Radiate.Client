using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class State<T> : IState<T>
{
    private T Value { get; set; }
    
    public State(T value)
    {
        Value = value;
    }
    
    public T GetValue() => Value;
    public IState GetState() => this;
    public string Name => GetType().Name;
    public void SetState(IState state) => SetState((T)state);

    public int ChangeCount { get; private set; }
    
    public event EventHandler<T>? SelectedValueChanged;
    public event EventHandler? StateChanged;

    public void Reduce(IReducer reducer, IAction action)
    {
        if (reducer is IReducer<T> tReducer)
        {
            SetState(tReducer.Reduce(Value, action));
        }
    }

    public IState<TK> SelectState<TK>(Func<T, TK> selector) 
    {
        var newState = new StateSelection<T, TK>(this, selector);
        
        return newState;
    }

    protected void SetState(T state)
    {
        if (state!.Equals(Value))
        {
            return;
        }
        
        Value = state;
        ChangeCount++;
        SelectedValueChanged?.Invoke(this, Value);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}

