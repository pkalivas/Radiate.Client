using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateSelection<T, TK> : IStateSelection<T, TK>
    where T : IState<T>
    where TK : IState<TK>
{
    private readonly IFeature<T> _feature;
    private readonly Func<T, TK> _selector;
    private TK? _previousState;
    
    public StateSelection(IFeature<T> feature, TK state, Func<T, TK> selector)
    {
        _feature = feature;
        _selector = selector;
        _previousState = state;
    }
    
    public TK State => _selector(_feature.State);

    public event EventHandler? StateChanged;
    public event EventHandler<TK>? SelectedValueChanged;
    
    public T1 Select<T1>(Func<TK, T1> selector) where T1 : IState<T1>
    {
        throw new NotImplementedException();
    }
    
    public void OnStateChanged(T val)
    {
        var newState = _selector(val);
        
        if (newState.Equals(_previousState))
        {
            return;
        }
        
        _previousState = newState;
        
        StateChanged?.Invoke(this, EventArgs.Empty);
        SelectedValueChanged?.Invoke(this, newState);
    }

    public void Dispose()
    {
        StateChanged = null;
        SelectedValueChanged = null;
    }
}