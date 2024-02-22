using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store.Reducers;

public static class RunUiReducers
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
    [
        Reducer.On<SetEngineTreeExpandedAction, RootState>(SetTreeExpansions),
        Reducer.On<RunUiCreatedAction, RootState>(AddRunUi),
        Reducer.On<SetPanelsExpandedAction, RootState>(SetPanelsExpanded),
        Reducer.On<SetAllPanelsExpandedAction, RootState>(SetAllPanelsExpanded)
    ];

    private static RootState AddRunUi(RootState state, RunUiCreatedAction action) => state
        .UpdateRunUi(action.RunUi.RunId, _ => action.RunUi with
        {
            PanelExpanded = action.RunUi.RunTemplate.UI.LeftSideAccordion.ExpansionPanels
                .Concat(action.RunUi.RunTemplate.UI.RightSideAccordion.ExpansionPanels)
                .ToImmutableDictionary(key => key.Id, _ => false)
        });
    
    private static RootState SetTreeExpansions(RootState state, SetEngineTreeExpandedAction action) => state
        .UpdateRunUi(action.RunId, ui => ui with
        {
            EngineStateExpanded = action.Expanded.ToImmutableDictionary()
        });
    
    private static RootState SetPanelsExpanded(RootState state, SetPanelsExpandedAction action) => state
        .UpdateRunUi(action.RunId, ui => ui with
        {
            PanelExpanded = ui.PanelExpanded
                .ToImmutableDictionary(key => key.Key, val => 
                    action.Expanded.TryGetValue(val.Key, out var expanded) ? expanded : val.Value)
        });

    private static RootState SetAllPanelsExpanded(RootState state, SetAllPanelsExpandedAction action) => state
        .UpdateRunUi(action.RunId, ui => ui with
        {
            PanelExpanded = ui.RunTemplate.UI.LeftSideAccordion.ExpansionPanels
                .Concat(ui.RunTemplate.UI.RightSideAccordion.ExpansionPanels)
                .ToImmutableDictionary(key => key.Id, _ => action.Expanded)
        });
}