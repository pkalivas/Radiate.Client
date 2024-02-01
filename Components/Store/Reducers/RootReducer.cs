using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Reducers;

public class RootReducer : Reducer<RootFeature>
{
    public override RootFeature Reduce(RootFeature state, IAction action) => action switch
    {
        NavigateToRunAction navigateToRunAction => state with { CurrentRunId = navigateToRunAction.RunId },
        RunCreatedAction runCreatedAction => state with
        {
            Runs = state.Runs.Values.Concat(new[] { runCreatedAction.Run }).ToDictionary(run => run.RunId),
        },
        _ => state
    };
}