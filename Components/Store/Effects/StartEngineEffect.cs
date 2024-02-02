using System.Reactive.Linq;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Redux;
using Redux.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public class StartEngineEffect : Effect<RootFeature, StartEngineAction>
{
    public StartEngineEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }
    
    public override async Task HandleAsync(RootFeature feature, StartEngineAction action, IDispatcher dispatcher)
    {
        // await using var scope = ServiceProvider.CreateAsyncScope();
        // var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();
        //
        // var currentRun = feature.Runs[action.RunId];
        //
        // actorService.Tell(new RunsActorMessage<StartRunCommand>(new StartRunCommand(currentRun.RunId, currentRun.Inputs)));
    }
}

public class TestE : IEffect<RootFeature>
{
    private readonly IServiceProvider _serviceProvider;

    public TestE(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        Run = store => store.ObserveAction<StartEngineAction>()
            .Do(action => HandleAsync(store.State, action))
            .Select(_ => new object());
    }

    public Func<Store<RootFeature>, IObservable<object>>? Run { get; set; }
    
    public bool Dispatch { get; set; }
    
    public void HandleAsync(RootFeature feature, object action)
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