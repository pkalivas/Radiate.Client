using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class RunUiSelectors
{
    public static readonly ISelector<RootState, RunUiState> SelectRunUiState =
        Selectors.Create<RootState, RunUiState>(state => state.RunUis.TryGetValue(state.CurrentRunId, out var runUi)
            ? runUi
            : new());

    public static ISelector<RootState, RunPanelState> SelectRunPanelState(Guid panelId) =>
        Selectors.Create<RootState, RunState, RunUiState, RunPanelState>(RunSelectors.SelectRun, SelectRunUiState,
            (runState, runUiState) =>
            {
                return runUiState.Panels.TryGetValue(panelId, out var panelState)
                    ? panelState
                    : new();
            });

    public record RunPanelProjection
    {
        public Guid RunId { get; init; } = Guid.NewGuid();
        public Guid PanelId { get; init; } = Guid.NewGuid();
        public string PanelKey { get; init; } = "";
    }
}