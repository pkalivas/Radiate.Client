using System.Collections.Immutable;
using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;
using Radiate.Client.Services.Schema;
using Reflow.Actions;
using Reflow.Interfaces;
using static Reflow.Effects.Effects;

namespace Radiate.Client.Domain.Store.Effects;

public class UiEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            EngineTreeEffect,
            RunCreatedEffect
        };

    private IEffect<RootState> RunCreatedEffect => CreateEffect<RootState>(state => state
        .OnAction<RunCreatedAction>()
        .Select(pair =>
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
        }), true);
    
    private IEffect<RootState> EngineTreeEffect => CreateEffect<RootState>(state => state
        .OnAction<SetRunOutputsAction>()
        .Select<(RootState, SetRunOutputsAction), IAction>(pair =>
        {
            var (state, action) = pair;
            if (state.RunUis.TryGetValue(action.RunId, out var runUi))
            {
                if (!runUi.EngineStateExpanded.Any())
                {
                    var treeExpansions = action.EngineOutputs.First().EngineStates.ToDictionary(val => val.Key, _ => true);
                    return new SetEngineTreeExpandedAction(action.RunId, treeExpansions);
                }
            }

            return new NoopAction();
        }), true);
}