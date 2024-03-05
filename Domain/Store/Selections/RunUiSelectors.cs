using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class RunUiSelectors
{
    public static readonly ISelector<RootState, RunUiState> SelectRunUiState =
        Selectors.Create<RootState, RunUiState>(state => state.RunUis.TryGetValue(state.CurrentRunId, out var runUi)
            ? runUi
            : new());
}