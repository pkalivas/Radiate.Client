using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Actions;
using Reflow.Effects;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Effects;

public class UiEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            EngineTreeEffect
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