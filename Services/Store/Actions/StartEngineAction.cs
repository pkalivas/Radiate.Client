using Radiate.Client.Services.Store.Models;

namespace Radiate.Client.Services.Store.Actions;

public record StartEngineAction(Guid RunId, RunInputsModel Inputs);
