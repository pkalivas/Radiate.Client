using Radiate.Client.Components.Store.Models;

namespace Radiate.Client.Components.Store.Actions;

public record StartEngineAction(Guid RunId, RunInputsModel Inputs);
