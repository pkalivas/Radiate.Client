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

    public event EventHandler<T>? SelectedValueChanged;
    public event EventHandler? StateChanged;

    public void Reduce(IReducer reducer, IAction action)
    {
        if (reducer is IReducer<T> tReducer)
        {
            SetState(tReducer.Reduce(Value, action));
        }
    }
    
    private void SetState(T state)
    {
        if (state.Equals(Value))
        {
            return;
        }
        
        Value = state;
        SelectedValueChanged?.Invoke(this, Value);
        StateChanged?.Invoke(this, EventArgs.Empty);
    }
}

