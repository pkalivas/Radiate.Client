namespace Radiate.Client.Components.Store.Actions;

public record SetEngineTreeExpandedAction(Guid RunId, Dictionary<string, bool> Expanded);
