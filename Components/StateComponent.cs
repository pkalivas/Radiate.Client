using Microsoft.AspNetCore.Components;
using Radiate.Client.Components.Store;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components;

public abstract class StateComponent<T, TState> : ComponentBase, IDisposable
    where T : StateComponent<T, TState>
    where TState : IFeature<TState>
{
    [Inject] protected IStore Store { get; set; } = default!;
    [Inject] protected IDispatcher Dispatcher { get; set; } = default!;
    protected TState State { get; set; } = default!;
    
    protected override Task OnInitializedAsync()
    {
        State = Store.GetState<TState>();
        Store.GetStateContainer<TState>().OnChange += () =>
        {
            State = Store.GetState<TState>();
            InvokeAsync(StateHasChanged);
        };
        
        OnStateInitialized();
        return base.OnInitializedAsync();
    }
    
    protected virtual void OnStateInitialized() { }
    
    public void Dispose()
    {
        Store.GetStateContainer<TState>().OnChange -= StateHasChanged;
        Store.UnsubscribeAll(this);
    }
    
    protected void Subscribe<TAction>(Action<TAction> callback) where TAction : IAction =>
        Store.Subscribe<TAction>(this, act => InvokeAsync(() =>
        {
            callback(act);
            StateHasChanged();
        }));
    
    protected void Dispatch<TAction>(TAction action)
        where TAction : IAction<AppFeature> => Dispatcher.Dispatch<TAction, AppFeature>(action);

    protected void Dispatch(Action<AppFeature> act) =>
        Store.Dispatch<FunctionalAction, AppFeature>(new FunctionalAction(act));
}