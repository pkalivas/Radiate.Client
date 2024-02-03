using System.Reactive.Linq;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public class RunOutputEffect : IEffect<RootState>
{
    public RunOutputEffect()
    {
        Run = store => store
            .ObserveAction<AddEngineOutputAction>()
            .Select(action => HandleAsync(store.State, action));
    }
    
    public Func<Reflow.Store<RootState>, IObservable<object>>? Run { get; set; }
    public bool Dispatch { get; set; } = true;
    
    private object HandleAsync(RootState state, object action)
    {
        if (action is AddEngineOutputAction addEngineOutputAction)
        {
            if (!state.UiFeature.EngineStateExpanded.ContainsKey(state.CurrentRunId))
            {
                var treeExpansions = addEngineOutputAction.EngineOutputs.EngineStates.ToDictionary(val => val.Key, _ => true);
                return new SetEngineTreeExpandedAction(state.CurrentRunId, treeExpansions);
            }
        }

        return new NoopAction();
    }
}


// if (addEngineOutputAction.EngineOutputs.Outputs.Any(val => val.Name == "Image"))
// {
//     var imageString = addEngineOutputAction.EngineOutputs.GetOutputValue<string>("Image");
//     var imageData = Image.Load<Rgba32>(Convert.FromBase64String(imageString));
// }