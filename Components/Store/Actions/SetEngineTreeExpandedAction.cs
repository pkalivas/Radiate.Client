using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.Actions;

public record SetEngineTreeExpandedAction(Guid RunId, Dictionary<string, bool> Expanded) : IAction<RootFeature>;
