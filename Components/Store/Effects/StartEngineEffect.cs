using System.Reactive.Linq;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public class StartEngineEffect : IEffect<RootFeature>
{
    private readonly IServiceProvider _serviceProvider;

    public StartEngineEffect(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Run = store => store
            .ObserveAction<StartEngineAction>()
            .Do(action => HandleAsync(store.State, action));
    }

    public Func<Reflow.Store<RootFeature>, IObservable<object>>? Run { get; set; }
    
    public bool Dispatch { get; set; }

    private void HandleAsync(RootFeature feature, object action)
    {
        if (action is StartEngineAction start)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();
            
            var currentRun = feature.Runs[start.RunId];
            
            actorService.Tell(new RunsActorMessage<StartRunCommand>(new StartRunCommand(currentRun.RunId, currentRun.Inputs)));
        }
    }
}