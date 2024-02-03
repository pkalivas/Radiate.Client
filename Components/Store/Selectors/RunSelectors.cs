using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Selectors;

public static class RunSelectors
{
    public static readonly ISelectorWithoutProps<RootFeature, RunState> SelectCurrentRun = 
            Reflow.Selectors.Selectors.CreateSelector<RootFeature, RunState>(state =>
            {
                if (state.Runs.TryGetValue(state.CurrentRunId, out var run))
                {
                    return run;
                }

                return new RunState();
            });
}