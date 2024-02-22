using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;


public record CreateNewRunAction(Guid RunId, string ModelType, string DataSetType) : IAction;

public record RunCreatedAction(RunState Run) : IAction;

public record SetRunOutputsAction(Guid RunId, List<RunOutputsState> EngineOutputs) : IAction;

public record SetRunInputsAction(Guid RunId, RunInputsState Inputs) : IAction;

public record CopyRunAction(Guid CopyRunId, Guid NewRunId) : IAction;
