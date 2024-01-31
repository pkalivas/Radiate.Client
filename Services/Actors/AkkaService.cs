using Akka.Actor;
using Akka.Configuration;
using Akka.DependencyInjection;

namespace Radiate.Client.Services.Actors;


public interface IActorService
{
    void Tell(object message);
}

public class ActorService : IHostedService, IActorService
{
    private ActorSystem _actorSystem = null!;
    private IActorRef _routerActor = null!;
    private readonly IServiceProvider _serviceProvider;

    private static readonly string Config = """
                                            akka
                                            {
                                                loglevel=WARNING,
                                                loggers=["Akka.Logger.Extensions.Logging.LoggingLogger, Akka.Logger.Extensions.Logging"]
                                            }
                                            """;

    public ActorService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void Tell(object message)
    {
        _routerActor.Tell(message);
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var bootstrap = BootstrapSetup.Create().WithConfig(ConfigurationFactory.ParseString(Config));
        var dependencyResolverSetup = DependencyResolverSetup.Create(_serviceProvider);
        var actorSystemSetup = bootstrap.And(dependencyResolverSetup);
        _actorSystem = ActorSystem.Create("workflows", actorSystemSetup);

        var topLevelActorProps = DependencyResolver.For(_actorSystem).Props<TopLevelActor>();
        _routerActor = _actorSystem.ActorOf(topLevelActorProps);
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await CoordinatedShutdown.Get(_actorSystem).Run(CoordinatedShutdown.ClrExitReason.Instance);
    }
}