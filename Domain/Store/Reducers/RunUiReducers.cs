using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Actions;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store.Reducers;

public static class RunUiReducers
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
    [
        Reducer.On<RunUiCreatedAction, RootState>(AddRunUi),
        Reducer.On<SetTrainTestTypeAction, RootState>(SetTrainTestType),
        Reducer.On<RunUiPanelsCreatedAction, RootState>(AddRunUiPanels)
    ];

    private static RootState AddRunUi(RootState state, RunUiCreatedAction action) => state
        .UpdateRunUi(action.RunUi.RunId, _ => action.RunUi);
    
    private static RootState SetTrainTestType(RootState state, SetTrainTestTypeAction action) => state
        .UpdateRunUi(action.RunId, ui => ui with
        {
            TrainTest = action.TrainTestType
        });
    
    private static RootState AddRunUiPanels(RootState state, RunUiPanelsCreatedAction action) => state
        .UpdateRunUi(action.RunId, ui => ui with
        {
            PanelStates = action.Panels.ToImmutableDictionary(p => p.Index),
        });
    
    
}