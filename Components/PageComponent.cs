using Microsoft.AspNetCore.Components;
using MudBlazor;
using Radiate.Client.Components.Panels;
using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Actions;
using Reflow.Interfaces;

namespace Radiate.Client.Components;

public abstract class PageComponent : ComponentBase
{
    [Parameter] public Guid RunId { get; set; } = default!;
    [Inject] protected IDialogService DialogService { get; set; } = default!;
    [Inject] protected NavigationManager Navigation { get; set; } = default!;
    [Inject] protected IStore<RootState> Store { get; set; } = default!;
    
    protected abstract string ModelType { get; }
    
    protected async Task OpenEngineTree()
    {
        var options = new DialogOptions
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
        };

        var dialogRef = await DialogService.ShowAsync<EngineTreePanel>("Engine Tree", options);
        await dialogRef.Result;
    }
    
    protected async Task CopyRun(Guid runId)
    {
        var newRunId = Guid.NewGuid();

        Store.Dispatch(new CopyRunAction(runId, newRunId));
        Store.Dispatch(new NavigateToRunAction(newRunId));
        
        Navigation.NavigateTo($"/runs/{runId}/{ModelType}");
    }
}