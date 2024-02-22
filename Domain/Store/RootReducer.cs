using Radiate.Client.Domain.Store.Reducers;
using Reflow.Reducers;

namespace Radiate.Client.Domain.Store;

public static class RootReducer
{
    public static IEnumerable<On<RootState>> CreateReducers() =>
        new List<IEnumerable<On<RootState>>>
        {
            UiReducers.CreateReducers(),
            RunReducers.CreateReducers(),
            RunUiReducers.CreateReducers()
        }
        .SelectMany(on => on);
}