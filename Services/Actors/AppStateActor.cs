using Akka.Actor;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Services.Actors.Commands;

namespace Radiate.Client.Services.Actors;

public class AppStateActor : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    
    public AppStateActor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        ReceiveAsync<AppStateActorMessage>(Handle);
    }
    
    private async Task Handle(AppStateActorMessage message)
    {
        await using var scope = _serviceProvider.CreateAsyncScope();
        var store = scope.ServiceProvider.GetRequiredService<IStore>();
        var reducer = scope.ServiceProvider.GetRequiredService<IReducer<AppState>>();
        
        var newState = reducer.Reduce(store.GetFeature<AppState>(), message.Action);
        
        store.Register(newState);
        store.GetAction<AppState>().NotifyStateChanged();
        // store.GetFeature<AppState>().NotifyStateChanged();
    }
}