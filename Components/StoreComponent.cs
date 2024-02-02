using Microsoft.AspNetCore.Components;
using Radiate.Client.Components.Store.States.Features;
using Redux.Selectors;

namespace Radiate.Client.Components;

public abstract class StoreComponent<TModel> : ComponentBase
{
    [Inject] protected Redux.Store<RootFeature> Store { get; set; } = default!;
    
    protected TModel? Model { get; private set; } = default!;
    
    protected override Task OnInitializedAsync()
    {
        Select().Subscribe(SetModel);
        return base.OnInitializedAsync();
    }

    protected abstract IObservable<TModel> Select();
    
    protected void Dispatch(object action) => Store.Dispatch(action);
    
    protected void Subscribe<TAction>(Action<TAction> callback) => Store
        .ObserveAction<TAction>()
        .Subscribe(callback);

    private void SetModel(TModel model)
    {
        Model = model;
        InvokeAsync(StateHasChanged);
    }
}