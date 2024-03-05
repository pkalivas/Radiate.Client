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
            CreateUiPanelsEffect,
            SetUiPanelStatesEffect
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
            
            return new RunUiPanelsCreatedAction(runUi.RunId, PanelMapper.Flatten(runUi.RunTemplate!.UI.Panels)
                .Select(panel =>  new PanelState
                {
                    RunId = runUi.RunId,
                    Index = panel.Id,
                    Children = panel.ChildPanels.Select(child => child.Id),
                    Panel = panel,
                    IsVisible = true,
                    IsExpanded = panel is not AccordionPanelItem item || item.Expanded
                })
                .ToArray());
        }), true);
    
    private IEffect<RootState> SetUiPanelStatesEffect => CreateEffect<RootState>(state => state
        .OnAction<SetUiPanelStatesAction>()
        .Select(pair =>
        {
            var (state, action) = pair;

            var panelStates = state.RunUis.TryGetValue(action.RunId, out var runUi)
                ? runUi.PanelStates.Values.ToDictionary(key => key.Index)
                : new Dictionary<Guid, PanelState>();

            foreach (var updatePanel in action.Panels)
            {
                if (panelStates.TryGetValue(updatePanel.Index, out var panel))
                {
                    panelStates[panel.Index] = panel with
                    {
                        IsVisible = updatePanel.IsVisible,
                        IsExpanded = updatePanel.IsExpanded
                    };                     
                }
            }
                
            return new UiPanelStateUpdatedAction(action.RunId, panelStates.Values.ToArray());
        }), true);
}