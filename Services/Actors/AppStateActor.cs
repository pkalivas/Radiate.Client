using Akka.Actor;
using Radiate.Client.Services.Actors.Commands;

namespace Radiate.Client.Services.Actors;

public class AppStateActor : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    
    public AppStateActor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        ReceiveAsync<IAppStateActorMessage>(Handle);
    }

    private async Task Handle(IAppStateActorMessage message)
    {
        dynamic dynamicMessage = message;
    }
}