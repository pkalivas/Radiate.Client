using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.Actions;

public record AddEngineOutputAction(RunOutputsState EngineOutputs) : IAction<RootFeature>;
