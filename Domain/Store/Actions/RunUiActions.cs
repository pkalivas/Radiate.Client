using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record RunUiCreatedAction(RunUiState RunUi) : IAction;

public record RunUiPanelsCreatedAction(Guid RunId, PanelState[] Panels) : IAction;

public record UiPanelStateUpdatedAction(Guid RunId, PanelState[] Panel) : IAction;

public record SetPanelsExpandedAction(Guid RunId, Guid[] PanelIds, bool IsExpanded) : IAction;
