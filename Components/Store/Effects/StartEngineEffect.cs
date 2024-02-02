using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;

namespace Radiate.Client.Components.Store.Effects;

public class StartEngineEffect : Effect<RootFeature, StartEngineAction>
{
    public StartEngineEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }
    
    public override async Task HandleAsync(RootFeature feature, StartEngineAction action, IDispatcher dispatcher)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();
        
        var currentRun = feature.Runs[action.RunId];
        
        actorService.Tell(new RunsActorMessage<StartRunCommand>(new StartRunCommand(currentRun.RunId, currentRun.Inputs)));
    }
}