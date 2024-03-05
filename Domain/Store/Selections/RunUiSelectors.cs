using Radiate.Client.Components.Panels.TemplatePanels;
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
    
    public static ISelector<RootState, TPanel> SelectPanel<TPanel>(Guid panelId) 
        where TPanel : IPanel => Selectors.Create<RootState, RunUiState, TPanel>(SelectRunUiState, runUi =>
            runUi.Panels.TryGetValue(panelId, out var panelState) ? (TPanel) panelState : default!);
    
    public static ISelector<RootState, PanelDisplayProjection> SelectPanelDisplayProjection(Guid panelId) =>
        Selectors.Create<RootState, RunUiState, PanelDisplayProjection>(SelectRunUiState, runUi =>
        {
            if (runUi.Panels.TryGetValue(panelId, out var panel))
            {
                var componentType = panel switch
                {
                    GridPanel => typeof(GridPanelDisplay),
                    GridPanel.GridItem => typeof(GridItemPanelDisplay),
                    ToolbarPanel => typeof(ToolbarPanelDisplay),
                    BoundedPaperPanel => typeof(BoundedPaperPanelDisplay),
                    TabPanel => typeof(TabPanelDisplay),
                    PaperPanel => typeof(PaperPanelDisplay),
                    CardPanel => typeof(CardPanelDisplay),
                    AccordionPanel => typeof(AccordionPanelDisplay),
                    AccordionPanelItem => typeof(ExpansionPanelDisplay),
                    DivPanel => typeof(DivPanelDisplay),
                    _ => throw new Exception($"Panel {panel.GetType()} not supported")
                };
                
                return new PanelDisplayProjection(panelId, componentType, new()
                {
                    ["PanelId"] = panelId,
                });
            }

            return new PanelDisplayProjection(panelId, typeof(IPanel), new());
        });
}