using Radiate.Client.Components.Store.Interfaces;

namespace Radiate.Client.Services.Actors.Commands;

public interface IAppStateMessage
{
    Guid StateId { get; }
}

public interface IAppStateActorMessage
{
    Guid StateId { get; }
}

public record AppStateActorMessage<TAction>(IAppStateMessage Action) : IAppStateActorMessage
    where TAction : IStateAction
{
    public Guid StateId => Action.StateId;
}
