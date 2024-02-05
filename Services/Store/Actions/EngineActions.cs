using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store.Actions;

public record StartEngineAction(Guid RunId, RunInputsState Inputs) : IAction;

public record PauseEngineRunAction(Guid RunId) : IAction;

public record ResumeEngineRunAction(Guid RunId) : IAction;

public record CancelEngineRunAction(Guid RunId) : IAction;

public record EngineStoppedAction : IAction;
