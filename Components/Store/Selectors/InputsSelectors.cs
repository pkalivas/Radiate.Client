using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;
using Reflow.Interfaces;

namespace Radiate.Client.Components.Store.Selectors;

public static class InputsSelectors
{
    public static ISelectorWithoutProps<RootState, RunInputsFeature> SelectCurrentRunInputs = 
        Reflow.Selectors.Selectors.CreateSelector<RootState, RunInputsFeature>(state =>
        {
            if (state.Runs.TryGetValue(state.CurrentRunId, out var run))
            {
                return run.Inputs;
            }

            return new RunInputsFeature();
        });
}