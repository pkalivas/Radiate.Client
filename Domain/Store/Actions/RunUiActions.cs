using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates.Panels;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record RunUiCreatedAction(RunUiState RunUi) : IAction;

public record RunUiPanelsCreatedAction(Guid RunId, IPanel[] Panels) : IAction;

public record UiPanelStateUpdatedAction(Guid RunId, IPanel[] Panels) : IAction;

public record SetPanelsExpandedAction(Guid RunId, Guid[] PanelIds, bool IsExpanded) : IAction;
