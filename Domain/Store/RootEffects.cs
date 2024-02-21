using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Reflow.Actions;
using Reflow.Effects;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store;

public class RootEffects : IEffectRegistry<RootState>
{
    private readonly IActorService _actorService;
    
    public RootEffects(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    public IEnumerable<IEffect<RootState>> CreateEffects() => new IEffect<RootState>[]
    {
        StartEngineEffect,
        CancelEngineEffect,
        EngineTreeEffect
    };

    private Effect<RootState> StartEngineEffect => new()
    {
        Run = state => state
            .OnAction<StartEngineAction>()
            .Select(pair =>
            {
                var (_, action) = pair;
                var (runId, inputs) = action;
                _actorService.Tell(new RunsActorMessage<StartRunCommand>(new StartRunCommand(runId, inputs)));

                return new NoopAction();
            })
    };
    
    private Effect<RootState> CancelEngineEffect => new()
    {
        Run = state => state
            .OnAction<CancelEngineRunAction>()
            .Select(pair =>
            {
                var (_, action) = pair;
                var runId = action.RunId;
                _actorService.Tell(new RunsActorMessage<StopRunCommand>(new StopRunCommand(runId)));
                
                return new NoopAction();
            })
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
