using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Actions;

public record FunctionalAction(Action<AppState> Act) : IAction<AppState>
{
}