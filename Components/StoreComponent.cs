using System.Reactive.Linq;
using Microsoft.AspNetCore.Components;
using Radiate.Client.Services.Store;
using Reflow;
using Reflow.Interfaces;

namespace Radiate.Client.Components;

public abstract class StoreComponent<TModel> : ComponentBase, IDisposable
{
    [Inject] protected IStore<RootState> Store { get; set; } = default!;
    
    protected TModel? Model { get; private set; }
    
    private readonly List<IDisposable> _subscriptions = new();

    protected bool IsLoading { get; set; } = false;
    
    protected override Task OnInitializedAsync()
    {
        _subscriptions.Add(Select()
            .Where(val => val != null)
            .DistinctUntilChanged()
            .Subscribe(SetModel));
        OnSubscribed();
        return base.OnInitializedAsync();
    }

    protected virtual IObservable<TModel> Select() => Observable.Empty<TModel>();
    
    protected virtual void OnSubscribed() { }
    
    protected void Dispatch<TAction>(TAction action) where TAction : IAction =>
        Store.Dispatch(action);

    protected void Subscribe<TAction>(Action<TAction> callback)
    {
        _subscriptions.Add(Store
            .OnAction<TAction>()
            .Select(pair => pair.Action)
            .Subscribe(callback));
    }

    protected virtual void SetModel(TModel model)
    {
        // Model = model;
        if (!IsLoading)
        {
            InvokeAsync(() =>
            {
                Model = model;
                StateHasChanged();
                
            });
        }
    }

    public void Dispose()
    {
        foreach (var subscription in _subscriptions)
        {
            subscription.Dispose();
        }
    }
}