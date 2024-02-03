using Radiate.Client.Services.Store.Models;

namespace Radiate.Client.Services.Store.Actions;

public record StartEngineAction(Guid RunId, RunInputsModel Inputs);

public record PauseEngineRunAction(Guid RunId);

public record ResumeEngineRunAction(Guid RunId);

public record CancelEngineRunAction(Guid RunId);

public record EngineStoppedAction;

public record AddEngineOutputAction(RunOutputsModel EngineOutputs);
