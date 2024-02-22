using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Actions;
using Reflow.Interfaces;
using static Reflow.Effects.Effects;

namespace Radiate.Client.Domain.Store.Effects;

public class RunUiEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            EngineTreeEffect,
            RunCreatedEffect
        };

    private IEffect<RootState> RunCreatedEffect => CreateEffect<RootState>(state => state
        .OnAction<RunUiCreatedAction>()
        .Select(pair => new List<IAction>
        {
            new SetRunLoadingAction(pair.Action.RunUi.RunId, false)
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