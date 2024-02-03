using System.Reactive.Linq;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public class CancelEngineEffect : IEffect<RootFeature>
{
    private readonly IServiceProvider _serviceProvider;
    
    public CancelEngineEffect(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Run = store => store
            .ObserveAction<CancelEngineRunAction>()
            .Select(async action => await HandleAsync(action));
    }
    public Func<Reflow.Store<RootFeature>, IObservable<object>>? Run { get; set; }
    public bool Dispatch { get; set; }

    private async Task HandleAsync(object action)
    {
        if (action is CancelEngineRunAction cancelEngineRunAction)
        {
            await using var scope = _serviceProvider.CreateAsyncScope();
            var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();

            actorService.Tell(new RunsActorMessage<StopRunCommand>(new StopRunCommand(cancelEngineRunAction.RunId)));
        }
    }
}