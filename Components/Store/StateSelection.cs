using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateSelection<T, TK> : IStateSelection<T, TK>
    where T : IState<T>
    where TK : IState<TK>
{
    private readonly IState<T> _state;
    private readonly Func<T, TK> _selector;
    private TK _previousState;
    
    public StateSelection(IState<T> state, Func<T, TK> selector)
    {
        _state = state;
        _selector = selector;
        _previousState = _selector((T)_state.GetState());

        _state.StateChanged += SetState!;
    }

    public TK State => _previousState;

    public event EventHandler? StateChanged;
    public event EventHandler<TK>? SelectedValueChanged;
    
    public State<T1> Select<T1>(Func<TK, T1> selector) where T1 : IState<T1> => 
        new State<T1>(selector(_previousState));
    
    public void Dispose()
    {
        _state.StateChanged -= SetState!;
        StateChanged = null;
        SelectedValueChanged = null;
    }

    public string Name => _state.Name;
    public IState GetState() => _state;

    public Type GetStateType() => typeof(TK);
    public void SetState(IState state)
    {
        _previousState = (TK)state;
        
        StateChanged?.Invoke(this, EventArgs.Empty);
        SelectedValueChanged?.Invoke(this, _previousState);
    }

    private void SetState(object sender, EventArgs args)
    {
        var newState = _selector((T)_state.GetState());
        
        if (newState.Equals(_previousState))
        {
            return;
        }

        SetState(newState);
    }
}