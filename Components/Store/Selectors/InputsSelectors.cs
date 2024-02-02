using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.Selectors;

public static class InputsSelectors
{
    public static IState<RunInputState> Select(StateStore store) => 
        store.GetState<RootFeature>()
            .SelectState(state =>
            {
                if (state.Runs.TryGetValue(state.CurrentRunId, out var run))
                {
                    return run.Inputs;
                }
                
                return new RunInputState();
            });    
}