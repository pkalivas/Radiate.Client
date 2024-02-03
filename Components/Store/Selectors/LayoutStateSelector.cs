using Radiate.Client.Components.Store.States.Features;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Selectors;

public class LayoutState : ICopy<LayoutState>
{
    public bool IsSidebarOpen { get; init; }
    
    public LayoutState Copy() => new()
    {
        IsSidebarOpen = IsSidebarOpen
    };
}

public static class LayoutStateSelector
{
    public static ISelectorWithoutProps<RootFeature, LayoutState> SelectLayoutState = 
        Reflow.Selectors.Selectors.CreateSelector<RootFeature, LayoutState>(state => new LayoutState
        {
            IsSidebarOpen = state.UiState.IsSidebarOpen
        });
}