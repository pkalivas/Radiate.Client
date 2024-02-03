using System.Reactive.Linq;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;
using Reflow;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public abstract class EffectBase<TState> where TState : class, new()
{
    private readonly IServiceProvider _serviceProvider;
    
    protected EffectBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    protected async Task CreateEffect(Action<AsyncServiceScope, Store<TState>>? createEffect)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var store = scope.ServiceProvider.GetRequiredService<Store<TState>>();
        
        createEffect?.Invoke(scope, store);
    }
}

public class StartEngineEffect : IEffect<RootState> 
{
    private readonly IServiceProvider _serviceProvider;

    public StartEngineEffect(IServiceProvider serviceProvider) 
    {
        _serviceProvider = serviceProvider;
        Run = store => store
            .ObserveAction<StartEngineAction>()
            .Do(action => HandleAsync(store.State, action));
    }
    
    public Func<Store<RootState>, IObservable<object>>? Run { get; set; }
    
    public bool Dispatch { get; set; }

    private void HandleAsync(RootState state, object action)
    {
        if (action is StartEngineAction start)
        {
            using var scope = _serviceProvider.CreateAsyncScope();
            var actorService = scope.ServiceProvider.GetRequiredService<IActorService>();
            
            var currentRun = state.Runs[start.RunId];
            
            actorService.Tell(new RunsActorMessage<StartRunCommand>(new StartRunCommand(currentRun.RunId, currentRun.Inputs)));
        }
    }
}