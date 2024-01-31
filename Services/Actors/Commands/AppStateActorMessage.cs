using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Services.Actors.Commands;

public interface IAppStateActorMessage
{
    public string StateName => nameof(AppState);
}

public record AppStateActorMessage(IAppStateAction Action) : IAppStateActorMessage
{
}
