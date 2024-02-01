using Microsoft.AspNetCore.Components;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using AppState = Radiate.Client.Components.Store.States.AppState;

namespace Radiate.Client.Components;

public abstract class StateComponent : ComponentBase, IDisposable
{
    [Inject] protected IStore Store { get; set; } = default!;
    protected AppState State { get; set; } = default!;
    
    protected override Task OnInitializedAsync()
    {
        State = Store.GetFeature<AppState>();
        Store.GetStateContainer<AppState>().OnChange += () =>
        {
            State = Store.GetFeature<AppState>();
            InvokeAsync(StateHasChanged);
        };
        return base.OnInitializedAsync();
    }
    
    public void Dispose()
    {
        Store.GetStateContainer<AppState>().OnChange -= StateHasChanged;
    }
    
    protected void Dispatch<TAction>(TAction action)
        where TAction : IAction<AppState, TAction> => Store.Dispatch<TAction, AppState>(action);

    protected void Dispatch(Action<AppState> act) =>
        Store.Dispatch<FunctionalAction, AppState>(new FunctionalAction(act));
}