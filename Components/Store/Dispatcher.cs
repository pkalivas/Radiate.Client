using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Services.Actors;
using Radiate.Client.Services.Actors.Commands;

namespace Radiate.Client.Components.Store;

public class Dispatcher : IDispatcher
{
    private readonly IActorService _actorService;
    
    public Dispatcher(IActorService actorService)
    {
        _actorService = actorService;
    }
    
    public void Dispatch(IStateAction stateAction)
    {
        var message = stateAction switch
        {
            IAppStateAction appStateAction => new AppStateActorMessage(appStateAction),
            _ => throw new NotImplementedException()
        };
        
        _actorService.Tell(message);
    }
}