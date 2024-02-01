using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Reducers;

public class AppStateReducer : RootReducer<AppState>
{
    public override AppState Reduce(AppState state, IAction action) => action switch
    {
        CountAction countAction => state with { Count = state.Count + 1 },
        StartEngineAction startEngineAction => state with
        {
            CancellationTokenSource = new(),
            Running = true,
            ModelType = startEngineAction.ModelType,
            DataSetType = startEngineAction.DataSetType
        },
        RunCreatedAction runCreatedAction => state with
        {
            Runs = state.Runs.Concat(new[] { runCreatedAction.Run }).ToList(),
            Running = false,
            CancellationTokenSource = new CancellationTokenSource(),
            Scores = new()
        },
        _ => state
    };
}