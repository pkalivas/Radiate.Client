using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Services;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Radiate.Engines.Schema;
using Reflow.Actions;
using Reflow.Interfaces;
using static Reflow.Effects.Effects;

namespace Radiate.Client.Domain.Store.Effects;

public class RunEffects : IEffectRegistry<RootState>
{
    private readonly IActorService _actorService;
    private readonly IValidationService _validationService;

    public RunEffects(IActorService actorService, IValidationService validationService)
    {
        _actorService = actorService;
        _validationService = validationService;
    }
    
    public IEnumerable<IEffect<RootState>> CreateEffects() => 
        new List<IEffect<RootState>>
        {
            StartEngineEffect,
            CancelEngineEffect,
            SetTargetImageInputEffect,
            ValidateDataSetEffect,
            SetScoresEffect
        };
    
    private IEffect<RootState> StartEngineEffect => CreateEffect<RootState>(store => store
        .OnAction<StartEngineAction>()
        .Select(pair =>
        {
            var (_, action) = pair;
            var (runId, inputs) = action;
            _actorService.Tell(new RunsActorMessage<StartRunCommand>(new StartRunCommand(runId, inputs)));
            
            return new NoopAction();
        }));
    
    private IEffect<RootState> CancelEngineEffect => CreateEffect<RootState>(state => state
        .OnAction<CancelEngineRunAction>()
        .Select(pair =>
        {
            var (_, action) = pair;
            var runId = action.RunId;
            _actorService.Tell(new RunsActorMessage<StopRunCommand>(new StopRunCommand(runId)));
            
            return new NoopAction();
        }));
    
    private IEffect<RootState> SetTargetImageInputEffect => CreateEffect<RootState>(state => state
        .OnAction<SetTargetImageAction>()
        .Select(pair =>
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
        }), true);

    private IEffect<RootState> ValidateDataSetEffect => CreateEffect<RootState>(store => store
        .OnAction<BatchRunOutputsAction>()
        .Select(pair =>
        {
            var (_, action) = pair;
            var (runId, outputs) = action;

            var lastOutput = outputs.Last();
            
            return new SetRunOutputsAction(runId, _validationService.Validate(runId, lastOutput));
        }), true);
    
    private IEffect<RootState> SetScoresEffect => CreateEffect<RootState>(store => store
        .OnAction<BatchRunOutputsAction>()
        .Select(pair =>
        {
            var (_, action) = pair;
            var (runId, outputs) = action;
            
            return new SetRunScoresAction(runId, outputs
                .Select(val => (float) val.Metrics[MetricNames.Score].Value)
                .ToList());
        }), true);
}