using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Components.Store.States;

public class Feature2<TState> : IFeature<TState>, IStateSelection<TState, TState>
    where TState : IState<TState>, IFeature<TState>
{
    private TState _state = default!;
    
    public Feature2(TState state)
    {
        _state = state;
    }
    
    public TState State => _state;
    public string Name => GetType().Name;
    public IState GetState() => State;
    public Type GetStateType() => typeof(TState);
    
    public void SetState(IState state)
    {
        if (state.Equals(_state))
        {
            return;
        }
        
        _state = (TState)state;
        
        StateChanged?.Invoke(this, EventArgs.Empty);
        SelectedValueChanged?.Invoke(this, _state);
    }

    public event EventHandler? StateChanged;
    public event EventHandler<TState>? SelectedValueChanged;
    public State<T> Select<T>(Func<TState, T> selector) where T : IState<T> => 
        new State<T>(selector(_state));

    public void Dispose()
    {
        StateChanged = null;
        SelectedValueChanged = null;
    }
}


public abstract record Feature<TState> : IFeature<TState>
    where TState : IFeature<TState>, IState<TState>
{
    public string Name => GetType().Name;
    public IState GetState() => State;
    public Type GetStateType() => typeof(TState);
    
    public void SetState(IState state)
    {
        throw new NotImplementedException();
    }

    public TState State { get; }
    public event EventHandler? StateChanged;
}
