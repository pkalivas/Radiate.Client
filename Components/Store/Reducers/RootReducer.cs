using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Reducers;

public class RootReducer : Reducer<RootFeature>
{
    public override RootFeature Reduce(RootFeature state, IAction action) => action switch
    {
        NavigateToRunAction navigateToRunAction => state with { CurrentRunId = navigateToRunAction.RunId },
        _ => state
    };
}