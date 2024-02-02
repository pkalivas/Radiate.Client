using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.Effects;

public class RunOutputEffect : Effect<RootFeature, AddEngineOutputAction>
{
    public RunOutputEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public override Task HandleAsync(RootFeature state, AddEngineOutputAction action, IDispatcher dispatcher)
    {
        if (!state.UiState.EngineStateExpanded.ContainsKey(state.CurrentRunId))
        {
            dispatcher.Dispatch<SetEngineTreeExpandedAction, RootFeature>(new SetEngineTreeExpandedAction(state.CurrentRunId, action.EngineOutputs.EngineStates
                .ToDictionary(key => key.Key, _ => true)));
        }
        
        return Task.CompletedTask;
    }
}