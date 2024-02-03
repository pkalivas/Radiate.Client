using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class StateSelectors
{
    public static readonly ISelector<RootState, RootState> SelectState = Selectors
        .Create<RootState, RootState>(state => state);
}