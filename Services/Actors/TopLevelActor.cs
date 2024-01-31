using Akka.Actor;
using Radiate.Client.Extensions;
using Radiate.Client.Services.Actors.Commands;

namespace Radiate.Client.Services.Actors;

public class TopLevelActor : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    
    public TopLevelActor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        Receive<IAppStateActorMessage>(RouteTo);
    }

    private void RouteTo(IAppStateActorMessage message)
    {
        var actor = GetIdentifiableChildByConvention<AppStateActor>(message.StateId);
        actor.Tell(message);
    }
    
    private IActorRef GetChildByConvention<T>() where T : ReceiveActor =>
        Context.GetChild(Props.Create<T>(_serviceProvider), $"{typeof(T).Name}");

    private IActorRef GetIdentifiableChildByConvention<T>(Guid id) where T : ReceiveActor =>
        Context.GetChild(Props.Create<T>(_serviceProvider, id), $"{typeof(T).Name}_{id}");

    private IActorRef GetIdentifiableChildByConvention<T>(string id) where T : ReceiveActor =>
        Context.GetChild(Props.Create<T>(_serviceProvider), id);
}