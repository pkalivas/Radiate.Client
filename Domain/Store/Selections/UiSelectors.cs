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
        .Create<RootState, StandardRunUiProjection>(state =>
        {
            if (state.RunUis.TryGetValue(state.CurrentRunId, out var runUi))
            {
                return new StandardRunUiProjection
                {
                    RunId = state.CurrentRunId,
                    IsLoading = false,
                    UiTemplate = runUi.RunTemplate!.UI
                };
            }
            
            return new StandardRunUiProjection
            {
                RunId = state.CurrentRunId,
                IsLoading = true,
                UiTemplate = null,
            };
        });

    public static readonly ISelector<RootState, ToolBarProjection> SelectToolBarModel = Selectors
        .Create<RootState, RunState, ToolBarProjection>(RunSelectors.SelectRun, run => new ToolBarProjection
        {
            RunId = run.RunId,
            ModelType = run.Inputs.ModelType
        });
}
        