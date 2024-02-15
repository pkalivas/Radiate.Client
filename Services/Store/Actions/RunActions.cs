using Radiate.Client.Services.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store.Actions;

public record RunCreatedAction(RunState Run) : IAction;

public record SetRunOutputsAction( RunOutputsState EngineOutputs) : IAction;

public record SetRunInputsAction(Guid RunId, RunInputsState Inputs) : IAction;

public record CopyRunAction(RunState Run) : IAction;
