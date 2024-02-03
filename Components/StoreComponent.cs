using System.Reactive.Linq;
using Microsoft.AspNetCore.Components;
using Radiate.Client.Services.Store;
using Reflow;

namespace Radiate.Client.Components;

public abstract class StoreComponent<TModel> : ComponentBase, IDisposable
{
    [Inject] protected Store<RootState> Store { get; set; } = default!;
    
    protected TModel? Model { get; private set; }
    
    private readonly List<IDisposable> _subscriptions = new();
    
    protected override Task OnInitializedAsync()
    {
        _subscriptions.Add(Select().Subscribe(SetModel));
        OnSubscribed();
        return base.OnInitializedAsync();
    }

    protected virtual IObservable<TModel> Select() => Observable.Empty<TModel>();
    
    protected virtual void OnSubscribed() { }
    
    protected void Dispatch(object action) => Store.Dispatch(action);

    protected void Subscribe<TAction>(Action<TAction> callback)
    {
        _subscriptions.Add(Store
            .ObserveAction<TAction>()
            .Subscribe(callback));
    }

    private void SetModel(TModel model)
    {
        Model = model;
        InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Dispose();
        }
    }
}