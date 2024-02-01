using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;
using Radiate.Engines.Schema;

namespace Radiate.Client.Components.Store.Reducers;

public class AppStateReducer : RootReducer<AppState>
{
    public override AppState Reduce(AppState state, IAction action) => action switch
    {
        StartEngineAction _ => state with
        {
            Running = true,
            CancellationTokenSource = new CancellationTokenSource()
        },
        RunCreatedAction runCreatedAction => state with
        {
            Runs = state.Runs.Concat(new[] { runCreatedAction.Run }).ToList(),
            Running = false,
            Scores = new()
        },
        AddEngineOutputAction engineOutputsGeneratedAction => state with
        {
            EngineOutputs = engineOutputsGeneratedAction.EngineOutputs,
            Scores = state.Scores.Concat(new[] { engineOutputsGeneratedAction.EngineOutputs.Metrics.Get(MetricNames.Score).Statistics.LastValue }).ToList(),
        },
        RunCompletedAction _ => state with
        {
            Running = false,
            CancellationTokenSource = null
        },
        _ => state
    };
}