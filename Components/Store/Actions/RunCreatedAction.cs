using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Actions;

public record RunCreatedAction(RunsState Run) : IAction<AppState>;