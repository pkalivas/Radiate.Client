using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record RunUiCreatedAction(RunUiState RunUi) : IAction;

public record RunUiPanelsCreatedAction(Guid RunId, RunPanelState[] Panels) : IAction;
