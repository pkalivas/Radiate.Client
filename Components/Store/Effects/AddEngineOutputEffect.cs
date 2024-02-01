using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;
using Radiate.Engines.Schema;

namespace Radiate.Client.Components.Store.Effects;

public class AddEngineOutputEffect : RootEffect<AppState, AddEngineOutputAction>
{
    public AddEngineOutputEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }
    
    public override Task HandleAsync(AppState state, AddEngineOutputAction action, IDispatcher dispatcher)
    {
        if (action.EngineOutputs.EngineState is EngineStateTypes.Stopped)
        {
            dispatcher.Dispatch<RunCompletedAction, AppState>(new RunCompletedAction());
        }
        
        return Task.CompletedTask;
    }
}