using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;
using Radiate.Engines.Schema;

namespace Radiate.Client.Components.Store.Reducers;

public class AppStateReducer : RootReducer<AppFeature>
{
    public override AppFeature Reduce(AppFeature feature, IAction action) => action switch
    {
        StartEngineAction _ => feature with
        {
            Running = true,
            CancellationTokenSource = new CancellationTokenSource()
        },
        RunCreatedAction runCreatedAction => feature with
        {
            Runs = feature.Runs.Concat(new[] { runCreatedAction.Run }).ToList(),
            Running = false,
            Scores = new()
        },
        AddEngineOutputAction engineOutputsGeneratedAction => feature with
        {
            EngineOutputs = engineOutputsGeneratedAction.EngineOutputs,
            Scores = feature.Scores.Concat(new[] { engineOutputsGeneratedAction.EngineOutputs.Metrics.Get(MetricNames.Score).Statistics.LastValue }).ToList(),
        },
        RunCompletedAction _ => feature with
        {
            Running = false,
            CancellationTokenSource = null
        },
        _ => feature
    };
}