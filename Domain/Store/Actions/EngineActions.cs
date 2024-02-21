using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record StartEngineAction(Guid RunId, RunInputsState Inputs) : IAction;

public record PauseEngineRunAction(Guid RunId) : IAction;

public record ResumeEngineRunAction(Guid RunId) : IAction;

public record CancelEngineRunAction(Guid RunId) : IAction;

public record EngineStoppedAction(Guid RunId) : IAction;
