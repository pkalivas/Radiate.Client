using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.Actions;

public record RunCreatedAction(RunState Run) : IAction<RootFeature>;