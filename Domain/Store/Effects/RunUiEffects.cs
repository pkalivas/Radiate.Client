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
            
            var flattenedPanels = PanelMapper.Flatten(runUi.RunTemplate!.UI.Panels);
            var panelIndexLookup = flattenedPanels
                .Select((panel, index) => (panel, index))
                .ToDictionary(pair => pair.panel.Id, pair => pair.index);

            return new RunUiPanelsCreatedAction(runUi.RunId, flattenedPanels.Select(panel => panel switch
            {
                AccordionPanelItem accPanelItem => new RunPanelState
                {
                    Visible = accPanelItem.Expanded,
                    Panel = accPanelItem,
                    PanelKey = accPanelItem.Id.ToString() + accPanelItem.Expanded,
                    Id = accPanelItem.Id,
                    Index = panelIndexLookup[accPanelItem.Id],
                    Children = accPanelItem.ChildPanels.Select(child => panelIndexLookup[child.Id])
                },
                _ => new RunPanelState
                {
                    Visible = true,
                    Panel = panel,
                    PanelKey = panel.Id.ToString(),
                    Id = panel.Id,
                    Index = panelIndexLookup[panel.Id],
                    Children = panel.ChildPanels.Select(child => panelIndexLookup[child.Id])
                }
            }).ToArray());
        }), true);
}