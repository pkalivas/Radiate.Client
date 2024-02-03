using Radiate.Client.Components.Store.States;
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
    public static ISelectorWithoutProps<RootState, LayoutState> SelectLayoutState = 
        Reflow.Selectors.Selectors.CreateSelector<RootState, LayoutState>(state => new LayoutState
        {
            IsSidebarOpen = state.UiFeature.IsSidebarOpen
        });
}