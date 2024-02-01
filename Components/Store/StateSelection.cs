using Radiate.Client.Components.Store.Interfaces;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store;

public class StateSelection<T, TK> : State<TK>, IStateSelection<T, TK>
    where T : ICopy<T>
    where TK : ICopy<TK>
{
    private readonly IState<T> _parent;
    private readonly Func<T, TK> _selector;
    private readonly TK _previousState;
    
    public StateSelection(IState<T> parent, Func<T, TK> selector) : base(selector(parent.GetValue()))
    {
        _parent = parent;
        _selector = selector;
        _previousState = _selector(_parent.GetValue());

        _parent.SelectedValueChanged += SetState!;
    }
    
    public IState<T1> Select<T1>(Func<TK, T1> selector) where T1 : ICopy<T1> =>
        new State<T1>(selector(_previousState));
    
    public void Dispose()
    {
        _parent.SelectedValueChanged -= SetState!;
    }
    
    public void SetState(object sender, T args)
    {
        var newState = _selector(_parent.GetValue());
        
        if (newState.Equals(_previousState))
        {
            return;
        }

        SetState(newState);
    }
}