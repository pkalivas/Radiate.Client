using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateSelection<T, TK> : State<TK>, IStateSelection<T, TK>
{
    private readonly IState<T> _state;
    private readonly Func<T, TK> _selector;
    private TK _previousState;
    
    public StateSelection(IState<T> state, Func<T, TK> selector) : base(selector(state.GetValue()))
    {
        _state = state;
        _selector = selector;
        _previousState = _selector(_state.GetValue());

        _state.StateChanged += SetState!;
    }

    public TK State => _previousState;
    
    public IState<T1> Select<T1>(Func<TK, T1> selector) => new State<T1>(selector(_previousState));
    
    public void Dispose()
    {
        _state.StateChanged -= SetState!;
    }
    
    public Type GetStateType() => typeof(TK);

    private void SetState(object sender, EventArgs args)
    {
        var newState = _selector((T)_state.GetValue());
        
        if (newState.Equals(_previousState))
        {
            return;
        }

        SetState(newState);
    }
}