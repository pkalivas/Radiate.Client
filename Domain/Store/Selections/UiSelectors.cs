using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class UiSelectors
{
    public static readonly ISelector<RootState, UiState> SelectUiState =
        Selectors.Create<RootState, UiState>(state => state.UiState);

    public static readonly ISelector<RootState, StandardRunUiProjection> SelectStandardRunUiModel = Selectors
        .Create<RootState, UiState, RunUiState, StandardRunUiProjection>(SelectUiState, RunUiSelectors.SelectRunUiState,
            (uiState, runUi) =>
        {
            if (runUi.RunTemplate is null)
            {
                return null;
            }
            
            var isLoading = uiState.LoadingStates.TryGetValue(runUi.RunId, out var loadingState) 
                ? loadingState 
                : true;
            
            if (runUi.RunTemplate != null)
            {
                // foreach (var panel in runUi.RunTemplate.UI.LeftSideAccordion.ExpansionPanels)
                // {
                //     panel.IsOpen = false;
                // }
            }

            return new StandardRunUiProjection
            {
                RunId = runUi.RunId,
                IsLoading = isLoading,
                UiTemplate = runUi.RunTemplate!.UI,
                IsExpanded = runUi.PanelExpanded
            };
        });

    public static readonly ISelector<RootState, ToolBarProjection> SelectToolBarModel = Selectors
        .Create<RootState, RunState, ToolBarProjection>(RunSelectors.SelectRun, run => new ToolBarProjection
        {
            RunId = run.RunId,
            ModelType = run.Inputs.ModelType
        });
}
        