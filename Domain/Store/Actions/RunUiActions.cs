using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record RunUiCreatedAction(RunUiState RunUi) : IAction;

public record SetEngineTreeExpandedAction(Guid RunId, Dictionary<string, bool> Expanded) : IAction;

public record SetPanelsExpandedAction(Guid RunId, Dictionary<Guid, bool> Expanded) : IAction;

public record SetAllPanelsExpandedAction(Guid RunId, bool Expanded) : IAction;