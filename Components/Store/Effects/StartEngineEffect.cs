using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Reducers;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Effects;

public class StartEngineEffect : RootEffect<AppState, StartEngineAction>
{
    public override Task HandleAsync(AppState state, StartEngineAction action, IDispatcher dispatcher)
    {
        throw new NotImplementedException();
    }
}