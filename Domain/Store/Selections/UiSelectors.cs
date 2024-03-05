using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates.Panels;
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
            if (runUi.RunTemplate is null || runUi.Panels.Count == 0)
            {
                return new StandardRunUiProjection
                {
                    RunId = runUi.RunId,
                    IsLoading = true,
                    PanelIds = new()
                };
            }
            
            return new StandardRunUiProjection
            {
                RunId = runUi.RunId,
                IsLoading = uiState.LoadingStates.GetValueOrDefault(runUi.RunId, true),
                PanelIds = runUi.RunTemplate.UI.Panels
                    .Select(panel => runUi.Panels[panel.Id].Id)
                    .ToList()
            };
        });

    public static readonly ISelector<RootState, ToolBarProjection> SelectToolBarModel = Selectors
        .Create<RootState, RunState, ToolBarProjection>(RunSelectors.SelectRun, run => new ToolBarProjection
        {
            RunId = run.RunId,
            ModelType = run.Inputs.ModelType,
            DataSetType = run.Inputs.DataSetType,
            IsRunning = run.IsRunning,
        });
    
    public static readonly ISelector<RootState, PanelStateDialogProjection> SelectPanelStateDialogModel = Selectors
        .Create<RootState, RunState, RunUiState, PanelStateDialogProjection>(RunSelectors.SelectRun, RunUiSelectors.SelectRunUiState,
            (runState, runUiState) => new PanelStateDialogProjection
        {
            RunId = runState.RunId,
            Panels = runUiState.Panels.Values
                .Select(panel => new PanelStateModel
                {
                    PanelId = panel.Id,
                    PanelType = panel.GetType().Name,
                    PanelName = panel.Title,
                    IsVisible = panel is not GridPanel.GridItem gridItem || gridItem.IsVisible,
                    IsExpanded = panel is not AccordionPanelItem accPanel || accPanel.Expanded,
                    Panel = panel
                })
                .ToList()
        });
    

}
        