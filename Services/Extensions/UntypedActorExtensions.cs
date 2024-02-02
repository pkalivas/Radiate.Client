using Akka.Actor;

namespace Radiate.Client.Services.Extensions;

public static class UntypedActorContextExtensions
{
    public static IActorRef GetChild(this IUntypedActorContext context, Props props, string name)
    {
        var child = context.Child(name);
        return child.IsNobody()
            ? context.ActorOf(props, name)
            : child;
    }
}
