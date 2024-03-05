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

            var t = PanelMapper.Flatten(runUi.RunTemplate!.UI.Panels)
                .Select(panel => new PanelState
                {
                    RunId = runUi.RunId,
                    Index = panel.Id,
                    Children = panel.ChildPanels.Select(child => child.Id),
                    Panel = panel,
                    IsVisible = true,
                    IsExpanded = panel is not AccordionPanelItem item || item.Expanded
                })
                .ToArray();
            
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
}