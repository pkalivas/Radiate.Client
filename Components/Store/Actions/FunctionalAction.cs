using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Actions;

public record FunctionalAction(Action<AppState> Act) : StateAction<AppState, FunctionalAction>
{
    public override Task Reduce(AppState feature)
    {
        Act(feature);
        return Task.CompletedTask;
    }
}