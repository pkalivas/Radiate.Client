using Akka.Actor;

namespace Radiate.Client.Extensions;

public static class ActorExtensions
{
    public static IActorRef GetChild(this IUntypedActorContext context, Props props, string name)
    {
        var child = context.Child(name);
        return child.IsNobody()
            ? context.ActorOf(props, name)
            : child;
    }
}