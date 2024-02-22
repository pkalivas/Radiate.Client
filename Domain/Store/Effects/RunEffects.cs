using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Reflow.Actions;
using Reflow.Effects;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Effects;

public class RunEffects : IEffectRegistry<RootState>
{
    private readonly IActorService _actorService;

    public RunEffects(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    public IEnumerable<IEffect<RootState>> CreateEffects() => 
        new List<IEffect<RootState>>
        {
            StartEngineEffect,
            CancelEngineEffect,
            SetTargetImageInputEffect
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
    
    private Effect<RootState> SetTargetImageInputEffect => new()
    {
        Run = store => store
            .OnAction<SetTargetImageAction>()
            .Select<(RootState, SetTargetImageAction), IAction>(pair =>
            {
                var (runId, image) = pair.Item2;
                var run = pair.Item1.Runs[runId];

                return new SetRunInputsAction(runId, run.Inputs with
                {
                    ImageInputs = run.Inputs.ImageInputs with
                    {
                        TargetImage = image
                    }
                });
            }),
        Dispatch = true
    };
}