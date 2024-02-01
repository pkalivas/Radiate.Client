using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateSelection<T, TK> : State<TK>, IStateSelection<T, TK>
{
    private readonly IState<T> _parent;
    private readonly Func<T, TK> _selector;
    private readonly TK _previousState;
    
    public StateSelection(IState<T> parent, Func<T, TK> selector) : base(selector(parent.GetValue()))
    {
        _parent = parent;
        _selector = selector;
        _previousState = _selector(_parent.GetValue());

        _parent.StateChanged += SetState!;
    }
    
    public IState<T1> Select<T1>(Func<TK, T1> selector) => new State<T1>(selector(_previousState));
    
    public void Dispose()
    {
        _parent.StateChanged -= SetState!;
    }
    
    private void SetState(object sender, EventArgs args)
    {
        var newState = _selector(_parent.GetValue());
        
        if (newState.Equals(_previousState))
        {
            return;
        }

        SetState(newState);
    }
}