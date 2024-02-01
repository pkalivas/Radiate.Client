using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;
using Radiate.Engines.Schema;

namespace Radiate.Client.Components.Store.Effects;

public class AddEngineOutputEffect : Effect<AppFeature, AddEngineOutputAction>
{
    public AddEngineOutputEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }
    
    public override Task HandleAsync(AppFeature feature, AddEngineOutputAction action, IDispatcher dispatcher)
    {
        if (action.EngineOutputs.EngineState is EngineStateTypes.Stopped)
        {
            dispatcher.Dispatch<RunCompletedAction, AppFeature>(new RunCompletedAction());
        }
        
        return Task.CompletedTask;
    }
}