using System.Collections.Immutable;
using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;
using Radiate.Client.Services.Schema;
using Reflow.Actions;
using Reflow.Effects;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Effects;

public class UiEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            EngineTreeEffect,
            PanelsExpandedEffect,
            RunCreatedEffect
        };

    private Effect<RootState> RunCreatedEffect => new()
    {
        Run = store => store
            .OnAction<RunCreatedAction>()
            .Select<(RootState, RunCreatedAction), IAction>(pair =>
            {
                var (_, action) = pair;
                var run = action.Run;
                
                IRunTemplate template = run.Inputs.ModelType switch
                {
                    ModelTypes.Graph => new GraphTemplate(),
                    ModelTypes.Image => new ImageTemplate(),
                    ModelTypes.Tree => new TreeTemplate(),
                    ModelTypes.MultiObjective => new MultiObjectiveTemplate(),
                    _ => throw new ArgumentOutOfRangeException()
                };

                return new RunUiCreatedAction(new RunUiState
                {
                    RunId = run.RunId,
                    RunTemplate = template,
                    PanelExpanded = template.UI.LeftSideAccordion.ExpansionPanels
                        .Concat(template.UI.RightSideAccordion.ExpansionPanels)
                        .ToImmutableDictionary(key => key.Id, val => val.IsOpen),
                });
            }),
        Dispatch = true
    };

    private Effect<RootState> PanelsExpandedEffect => new()
    {
        Run = store => store
            .OnAction<SetPanelsExpandedAction>()
            .Select<(RootState, SetPanelsExpandedAction), IAction>(pair =>
            {
                var (state, action) = pair;
                var (runId, expanded) = action;

                var template = state.UiState.RunTemplates[runId];

                for (var i = 0; i < template.UI.LeftSideAccordion.ExpansionPanels.Count; i++)
                {
                    var currentPanel = template.UI.LeftSideAccordion.ExpansionPanels[i];
                    
                    if (expanded.TryGetValue(currentPanel.Id, out var isOpen))
                    {
                        currentPanel.IsOpen = isOpen;
                    }
                }

                // foreach (var panel in template.UI.LeftSideAccordion.ExpansionPanels)
                // {
                //     if (expanded.TryGetValue(panel.Id, out var isOpen))
                //     {
                //         panel.IsOpen = isOpen;
                //     }
                // }
                //
                // foreach (var panel in template.UI.RightSideAccordion.ExpansionPanels)
                // {
                //     if (expanded.TryGetValue(panel.Id, out var isOpen))
                //     {
                //         panel.IsOpen = isOpen;
                //     }
                // }

                return new SetRunTemplateAction(runId, template);
            }),
        Dispatch = true
    };
    
    private Effect<RootState> EngineTreeEffect => new()
    {
        Run = store => store
            .OnAction<SetRunOutputsAction>()
            .Select<(RootState, SetRunOutputsAction), IAction>(pair =>
            {
                var (state, action) = pair;
                if (!state.UiState.EngineStateExpanded.ContainsKey(state.CurrentRunId))
                {
                    var treeExpansions = action.EngineOutputs.First().EngineStates.ToDictionary(val => val.Key, _ => true);
                    return new SetEngineTreeExpandedAction(state.CurrentRunId, treeExpansions);
                }

                return new NoopAction();
            }),
        Dispatch = true
    };
}