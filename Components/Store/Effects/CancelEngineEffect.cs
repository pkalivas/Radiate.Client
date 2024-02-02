using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;

namespace Radiate.Client.Components.Store.Effects;

public class CancelEngineEffect : Effect<RootFeature, CancelEngineRunAction>
{
    public CancelEngineEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public override async Task HandleAsync(RootFeature state, CancelEngineRunAction action, IDispatcher dispatcher)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();
        
        actorService.Tell(new RunsActorMessage<StopRunCommand>(new StopRunCommand(action.RunId)));
    }
}