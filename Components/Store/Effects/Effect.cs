namespace Radiate.Client.Components.Store.Effects;

// public abstract class Effect<TState, TAction> : IEffect<TState, TAction>
//     where TState : class, ICopy<TState>
//     where TAction : IAction
// {
//     protected readonly IServiceProvider ServiceProvider;
//
//     protected Effect(IServiceProvider serviceProvider)
//     {
//         ServiceProvider = serviceProvider;
//     }
//
//     public abstract Task HandleAsync(TState state, TAction action, IDispatcher dispatcher);
//
//     public bool CanHandle(IState feature, IAction action) => feature is IState<TState> && action is TAction;
//
//     public Task HandleAsync(IState feature, IAction action, IDispatcher dispatcher)
//     {
//         if (feature is IState<TState> tState && action is TAction tAction)
//         {
//             return HandleAsync(tState.GetValue(), tAction, dispatcher);
//         }
//
//         return Task.CompletedTask;
//     }
// }
//
// public static class TestEffects
// {
//     public static Effect<RootFeature> EngineOutputEffect = new()
//     {
//         Run = store => store.ObserveAction<AddEngineOutputAction>(),
//             // .Where(state => s)
//         Dispatch = true
//     };
//     // {
//     //     if (!state.UiState.EngineStateExpanded.ContainsKey(state.CurrentRunId))
//     //     {
//     //         var treeExpansions = action.EngineOutputs.EngineStates.ToDictionary(val => val.Key, _ => true);
//     //         dispatcher.Dispatch<SetEngineTreeExpandedAction, RootFeature>(new SetEngineTreeExpandedAction(state.CurrentRunId, treeExpansions));
//     //     }
//     //
//     //     if (action.EngineOutputs.Outputs.Any(val => val.Name == "Image"))
//     //     {
//     //         var imageString = action.EngineOutputs.GetOutputValue<string>("Image");
//     //         var imageData = SixLabors.ImageSharp.Image.Load<SixLabors.ImageSharp.PixelFormats.Rgba32>(Convert.FromBase64String(imageString));
//     //     }
//     //     
//     //     return Task.CompletedTask;
//     // });
// }