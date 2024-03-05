using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Mappers;
using Reflow.Interfaces;
using static Reflow.Effects.Effects;

namespace Radiate.Client.Domain.Store.Effects;

public class RunUiEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            RunCreatedEffect,
            CreateUiPanelsEffect,
            OpenAllPanelsEffect,
            SetUiPanelsExpandedEffect
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
            
            return new RunUiPanelsCreatedAction(runUi.RunId, PanelMapper.Flatten(runUi.RunTemplate!.UI.Panels).ToArray());
        }), true);
    
    private IEffect<RootState> SetUiPanelsExpandedEffect => CreateEffect<RootState>(store => store
        .OnAction<SetPanelsExpandedAction>()
        .Select(pair =>
        {
            var (state, action) = pair;
            var (runId, panelIds, isExpanded) = action;

            var panelStates = state.RunUis.TryGetValue(runId, out var runUi)
                ? runUi.Panels.Values.ToDictionary(key => key.Id)
                : new Dictionary<Guid, IPanel>();

            foreach (var panelId in panelIds)
            {
                if (panelStates.TryGetValue(panelId, out var panel))
                {
                    panelStates[panel.Id] = panel switch
                    {
                        AccordionPanelItem accordionPanelItem => accordionPanelItem with
                        {
                            Expanded = isExpanded
                        },
                        _ => panel
                    };
                }
            }
                
            return new UiPanelStateUpdatedAction(action.RunId, panelStates.Values.ToArray());
        }), true);
    
    private IEffect<RootState> OpenAllPanelsEffect => CreateEffect<RootState>(store => store
        .OnAction<StartEngineAction>()
        .Select(pair =>
        {
            var (state, action) = pair;

            var panelStates = state.RunUis.TryGetValue(action.RunId, out var runUi)
                ? runUi.Panels.Values.ToDictionary(key => key.Id)
                : new Dictionary<Guid, IPanel>();

            foreach (var panelId in panelStates.Keys)
            {
                if (panelStates.TryGetValue(panelId, out var panel))
                {
                    panelStates[panel.Id] = panel switch
                    {
                        GridPanel.GridItem gridItem => gridItem with
                        {
                            IsVisible = true,
                        },
                        AccordionPanelItem accordionPanelItem => accordionPanelItem with
                        {
                            Expanded = true
                        },
                        _ => panel
                    };
                }
            }
                
            return new UiPanelStateUpdatedAction(action.RunId, panelStates.Values.ToArray());
        }), true);
}