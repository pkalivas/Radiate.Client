using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.States;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store.Selections;

public static class LayoutStateSelector
{
    public static ISelectorWithoutProps<RootState, LayoutModel> SelectLayoutState = 
        Reflow.Selectors.Selectors.CreateSelector<RootState, LayoutModel>(state => new LayoutModel
        {
            IsSidebarOpen = state.UiFeature.IsSidebarOpen
        });
}