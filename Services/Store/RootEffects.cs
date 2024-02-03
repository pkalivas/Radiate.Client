using System.Reactive.Linq;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Radiate.Client.Services.Store.Actions;
using Reflow.Effects;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store;

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
        RunOutputEffect
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
    
    private Effect<RootState> RunOutputEffect => new()
    {
        Run = store => store
            .OnAction<AddRunOutputAction>()
            .Throttle(TimeSpan.FromMilliseconds(100))
            .Select<(RootState, AddRunOutputAction), IAction>(pair =>
            {
                var (state, action) = pair;
                if (!state.UiModel.EngineStateExpanded.ContainsKey(state.CurrentRunId))
                {
                    var treeExpansions = action.EngineOutputs.EngineStates.ToDictionary(val => val.Key, _ => true);
                    return new SetEngineTreeExpandedAction(state.CurrentRunId, treeExpansions);
                }

                return new NoopAction();
            }),
        Dispatch = true
    };
}
