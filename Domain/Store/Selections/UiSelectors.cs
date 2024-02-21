using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class UiSelectors
{
    public static readonly ISelector<RootState, UiState> SelectUiState =
        Selectors.Create<RootState, UiState>(state => state.UiState);
    
    public static readonly ISelector<RootState, IRunTemplate> SelectRunTemplate = Selectors
        .Create<RootState, IRunTemplate>(state => state.UiState.RunTemplates[state.CurrentRunId]);
}