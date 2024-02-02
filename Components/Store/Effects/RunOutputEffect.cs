using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Components.Store.Effects;

public class RunOutputEffect : Effect<RootFeature, AddEngineOutputAction>
{
    public RunOutputEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public override Task HandleAsync(RootFeature state, AddEngineOutputAction action, IDispatcher dispatcher)
    {
        if (!state.UiState.EngineStateExpanded.ContainsKey(state.CurrentRunId))
        {
	        var treeExpansions = action.EngineOutputs.EngineStates.ToDictionary(val => val.Key, _ => true);
            dispatcher.Dispatch<SetEngineTreeExpandedAction, RootFeature>(new SetEngineTreeExpandedAction(state.CurrentRunId, treeExpansions));
        }

        if (action.EngineOutputs.Outputs.Any(val => val.Name == "Image"))
        {
	        var imageString = action.EngineOutputs.GetOutputValue<string>("Image");
	        var imageData = Image.Load<Rgba32>(Convert.FromBase64String(imageString));
        }
        
        return Task.CompletedTask;
    }
}