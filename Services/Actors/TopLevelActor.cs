using Akka.Actor;
using Radiate.Client.Services.Actors.Commands;
using Radiate.Client.Services.Extensions;

namespace Radiate.Client.Services.Actors;

public class TopLevelActor : ReceiveActor
{
    private readonly IServiceProvider _serviceProvider;
    
    public TopLevelActor(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        
        Receive<IRunsActorMessage>(RouteTo);
    }
    
    private void RouteTo(IRunsActorMessage command)
    {
        var actor = GetChildByConvention<RunsActor>(command.RunId);
        actor.Tell(command);
    }

    private IActorRef GetChildByConvention<T>() where T : ReceiveActor =>
        Context.GetChild(Props.Create<T>(_serviceProvider), $"{typeof(T).Name}");
    
    private IActorRef GetChildByConvention<T>(Guid id) where T : ReceiveActor =>
        Context.GetChild(Props.Create<T>(_serviceProvider), $"{typeof(T).Name}_{id}");

    private IActorRef GetChildByConvention<T>(string id) where T : ReceiveActor =>
        Context.GetChild(Props.Create<T>(_serviceProvider), $"{typeof(T).Name}_{id}");
}