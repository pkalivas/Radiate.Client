using Microsoft.AspNetCore.Components;
using Radiate.Client.Components.Store;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components;

public abstract class StateComponent<T, TState> : ComponentBase, IDisposable
    where T : StateComponent<T, TState>
{
    [Inject] protected IStore Store { get; set; } = default!;
    [Inject] protected IDispatcher Dispatcher { get; set; } = default!;
    protected TState State { get; set; } = default!;
    
    protected int Changes { get; private set; }
    
    private IState<TState> _state = default!;
    
    protected override Task OnInitializedAsync()
    {
        _state = Store.Select<TState>();
        State = _state.GetValue();
        _state.SelectedValueChanged += SetState!;
        
        OnStateInitialized();
        return base.OnInitializedAsync();
    }
    
    protected virtual void OnStateInitialized() { }
    
    public void Dispose()
    {
        _state.SelectedValueChanged -= SetState!;
        Store.UnsubscribeAll(this);
    }
    
    protected void Subscribe<TAction>(Action<TAction> callback) where TAction : IAction =>
        Store.Subscribe<TAction>(this, act => InvokeAsync(() =>
        {
            callback(act);
            StateHasChanged();
        }));

    private void SetState(object sender, TState state)
    {
        State = state;
        Changes++;
        InvokeAsync(StateHasChanged);
    }
}