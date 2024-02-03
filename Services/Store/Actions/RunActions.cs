using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store.Actions;

public record RunCreatedAction(RunModel Run) : IAction;

public record AddRunOutputAction(RunOutputsModel EngineOutputs) : IAction;
