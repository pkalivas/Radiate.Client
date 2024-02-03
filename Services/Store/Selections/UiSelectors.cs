using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class UiSelectors
{
    public static readonly ISelector<RootState, UiModel> SelectUiState =
        Selectors.Create<RootState, UiModel>(state => state.UiModel);
}