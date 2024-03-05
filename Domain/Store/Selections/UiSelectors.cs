using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
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
            if (runUi.RunTemplate is null)
            {
                return null;
            }
            
            var isLoading = uiState.LoadingStates.TryGetValue(runUi.RunId, out var loadingState) 
                ? loadingState 
                : true;

            var orderedPanels = runUi.Panels.Values.OrderBy(val => val.Index).ToArray();
            
            return new StandardRunUiProjection
            {
                RunId = runUi.RunId,
                IsLoading = isLoading,
                UiTemplate = runUi.RunTemplate!.UI,
                Panels = runUi.RunTemplate.UI.Panels
                    .SelectMany(panel => TreeItemMapper.ToTree(runUi.Panels[panel.Id].Index, orderedPanels.ToDictionary(key => key.Index)))
                    .ToHashSet(),
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
        