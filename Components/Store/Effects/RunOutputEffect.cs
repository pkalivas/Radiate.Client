using System.Reactive.Linq;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States.Features;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Effects;

public class RunOutputEffect : IEffect<RootFeature>
{
    public RunOutputEffect()
    {
        Run = store => store
            .ObserveAction<AddEngineOutputAction>()
            .Select(action => HandleAsync(store.State, action));
    }
    
    public Func<Reflow.Store<RootFeature>, IObservable<object>>? Run { get; set; }
    public bool Dispatch { get; set; } = true;
    
    private IAction HandleAsync(RootFeature feature, object action)
    {
        if (action is AddEngineOutputAction addEngineOutputAction)
        {
            if (!feature.UiState.EngineStateExpanded.ContainsKey(feature.CurrentRunId))
            {
                var treeExpansions = addEngineOutputAction.EngineOutputs.EngineStates.ToDictionary(val => val.Key, _ => true);
                return new SetEngineTreeExpandedAction(feature.CurrentRunId, treeExpansions);
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