using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates.Panels;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class RunUiSelectors
{
    public static readonly ISelector<RootState, RunUiState> SelectRunUiState =
        Selectors.Create<RootState, RunUiState>(state => state.RunUis.TryGetValue(state.CurrentRunId, out var runUi)
            ? runUi
            : new());
    
    public static ISelector<RootState, IPanel> SelectPanel(Guid panelId) =>
        Selectors.Create<RootState, RunUiState, IPanel>(SelectRunUiState, runUi =>
            runUi.Panels.TryGetValue(panelId, out var panelState) ? panelState : default!);
}