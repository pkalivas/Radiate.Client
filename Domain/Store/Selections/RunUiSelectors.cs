using Radiate.Client.Domain.Store.Models.Projections;
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
    
    // public static ISelector<RootState, PanelStateProjection> SelectPanelModel(Guid panelId) => Selectors
    //     .Create<RootState, RunState, RunUiState, PanelStateProjection>(RunSelectors.SelectRun, RunUiSelectors.SelectRunUiState,
    //         (runState, runUiState) => new PanelStateProjection
    //         {
    //             RunId = runState.RunId,
    //             PanelId = runUiState.PanelTree[panelId].Index,
    //             Children = runUiState.PanelTree[panelId].Children
    //                 .Select(child => new PanelStateChild
    //                 {
    //                     RunId = runState.RunId,
    //                     PanelId = child,
    //                     IsVisible = true,
    //                     IsExpanded = runUiState.Panels[child].Panel is not AccordionPanelItem item || item.Expanded,
    //                 })
    //                 .ToList(),
    //             Panel = runUiState.Panels[panelId].Panel,
    //             IsVisible = true,
    //             IsExpanded = runUiState.Panels[panelId].Panel is not AccordionPanelItem item || item.Expanded,
    //         });
}