using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;
using Redux.Interfaces;
using Redux.Selectors;

namespace Radiate.Client.Components.Store.Selectors;

public static class RunSelectors
{
    public static IState<RunState> Select(StateStore store) => 
        store.GetState<RootFeature>().SelectState(state =>
        {
            if (state.Runs.TryGetValue(state.CurrentRunId, out var run))
            {
                return run;
            }
            
            return new RunState();
        });

    public static ISelectorWithoutProps<RootFeature, RunState> SelectCurrentRun = 
            Redux.Selectors.Selectors.CreateSelector<RootFeature, RunState>(state =>
            {
                if (state.Runs.TryGetValue(state.CurrentRunId, out var run))
                {
                    return run;
                }

                return new RunState();
            });
}