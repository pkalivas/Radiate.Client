using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

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
    public static IState<LayoutState> Select(StateStore store) => 
        store.GetState<RootFeature>()
            .SelectState(feature => feature.UiState)
            .SelectState(state => new LayoutState
            {
                IsSidebarOpen = state.IsSidebarOpen
            });
}