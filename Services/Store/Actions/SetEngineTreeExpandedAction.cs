namespace Radiate.Client.Services.Store.Actions;

public record SetEngineTreeExpandedAction(Guid RunId, Dictionary<string, bool> Expanded);
