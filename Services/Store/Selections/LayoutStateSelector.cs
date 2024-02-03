using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store.Selections;

public static class LayoutStateSelector
{
    public static ISelector<RootState, LayoutModel> SelectLayoutState = 
        Reflow.Selectors.Selectors.CreateSelector<RootState, LayoutModel>(state => new LayoutModel
        {
            IsSidebarOpen = state.UiFeature.IsSidebarOpen
        });
}