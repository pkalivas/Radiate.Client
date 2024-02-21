using Radiate.Client.Domain.Store.Models;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class LayoutStateSelector
{
    public static readonly ISelector<RootState, LayoutModel> SelectLayoutState = Selectors
        .Create<RootState, LayoutModel>(state => new LayoutModel
        {
            IsSidebarOpen = state.UiState.IsSidebarOpen
        });
}