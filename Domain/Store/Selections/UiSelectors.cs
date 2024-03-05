using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Mappers;
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
            if (runUi.RunTemplate is null || runUi.PanelStates.Count == 0)
            {
                return null;
            }
            
            var isLoading = uiState.LoadingStates.GetValueOrDefault(runUi.RunId, true);
            var orderedPanels = runUi.PanelStates.Values.OrderBy(val => val.Key).ToArray();

            return new StandardRunUiProjection
            {
                RunId = runUi.RunId,
                IsLoading = isLoading,
                UiTemplate = runUi.RunTemplate!.UI,
                PanelStates = runUi.RunTemplate.UI.Panels
                    .SelectMany(panel => TreeItemMapper.ToTree(runUi.PanelStates, panel.Id))
                    .ToArray(),
                OrderedPanels = orderedPanels
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
    

}
        