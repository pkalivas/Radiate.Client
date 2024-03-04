using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Mappers;
using Reflow.Actions;
using Reflow.Interfaces;
using static Reflow.Effects.Effects;

namespace Radiate.Client.Domain.Store.Effects;

public class RunUiEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            RunCreatedEffect,
            CreateUiPanelsEffect
        };

    private IEffect<RootState> RunCreatedEffect => CreateEffect<RootState>(state => state
        .OnAction<RunUiCreatedAction>()
        .Select(pair => new SetRunLoadingAction(pair.Action.RunUi.RunId, false)), true);

    private IEffect<RootState> CreateUiPanelsEffect => CreateEffect<RootState>(state => state
        .OnAction<RunUiCreatedAction>()
        .Select(pair =>
        {
            var (_, action) = pair;
            var runUi = action.RunUi;
            
            var flattenedPanels = PanelMapper.Flatten(runUi.RunTemplate!.UI.Panels, panel => panel switch
            {
                AccordionPanelItem accPanelItem => new RunPanelState
                {
                    Visible = true,
                    Panel = accPanelItem,
                    PanelKey = accPanelItem.Id.ToString() + accPanelItem.Expanded,
                    Id = accPanelItem.Id
                },
                _ => new RunPanelState
                {
                    Visible = true,
                    Panel = panel,
                    PanelKey = panel.Id.ToString(),
                    Id = panel.Id
                }
            });
            var panelLookup = flattenedPanels.ToDictionary(key => key.Id);
            var panelIndexLookup = flattenedPanels
                .Select((panel, index) => (panel, index))
                .ToDictionary(pair => pair.panel.Id, pair => pair.index);

            foreach (var panel in flattenedPanels)
            {
            }
            
            

            return new RunUiPanelsCreatedAction(runUi.RunId, CreatePanels(runUi).ToArray());
        }), true);
    
    private static List<RunPanelState> CreatePanels(RunUiState runUi)
    {
        var result = new List<RunPanelState>();

        foreach (var panel in runUi.RunTemplate!.UI.Panels)
        {
            result.AddRange(FlattenPanel(panel));
        }

        return result;
    }

    private static IEnumerable<RunPanelState> FlattenPanel(IPanel panel)
    {
        var result = new List<RunPanelState>();
        
        if (panel is GridPanel gridPanel)
        {
            result.Add(new RunPanelState
            {
                Visible = true,
                Panel = panel,
                PanelKey = panel.Id.ToString(),
                Id = panel.Id
            });
            
            foreach (var child in gridPanel.Items.Select(i => i.Panel))
            {
                result.AddRange(FlattenPanel(child));
            }
        }
        else if (panel is TabPanel tabPanel)
        {
            result.Add(new RunPanelState
            {
                Visible = true,
                Panel = panel,
                PanelKey = panel.Id.ToString(),
                Id = panel.Id
            });
            
            foreach (var child in tabPanel.ChildPanels)
            {
                result.AddRange(FlattenPanel(child));
            }
        }
        else if (panel is AccordionPanel accordionPanel)
        {
            result.Add(new RunPanelState
            {
                Visible = true,
                Panel = panel,
                PanelKey = panel.Id.ToString(),
                Id = panel.Id
            });
            
            foreach (var child in accordionPanel.ChildPanels)
            {
                result.AddRange(FlattenPanel(child));
            }
        }
        else if (panel is AccordionPanelItem accPanelItem)
        {
            result.Add(new RunPanelState
            {
                Visible = accPanelItem.Expanded,
                Panel = panel,
                PanelKey = panel.Id.ToString() + accPanelItem.Expanded,
                Id = panel.Id
            });
        }
        else
        {
            result.Add(new RunPanelState
            {
                Visible = true,
                Panel = panel,
                PanelKey = panel.Id.ToString(),
                Id = panel.Id
            });
        }
        
        return result;
    }
}