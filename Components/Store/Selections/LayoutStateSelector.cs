using Radiate.Client.Components.Store.Models;
using Radiate.Client.Components.Store.States;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Selections;

public static class LayoutStateSelector
{
    public static ISelectorWithoutProps<RootState, LayoutModel> SelectLayoutState = 
        Reflow.Selectors.Selectors.CreateSelector<RootState, LayoutModel>(state => new LayoutModel
        {
            IsSidebarOpen = state.UiFeature.IsSidebarOpen
        });
}