using Radiate.Client.Domain.Templates;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record LayoutChangedAction : IAction
{
    public bool IsSidebarOpen { get; init; }
}

public record NavigateToRunAction(Guid RunId) : IAction;

public record SetEngineTreeExpandedAction(Guid RunId, Dictionary<string, bool> Expanded) : IAction;

public record SetPanelsExpandedAction(Guid RunId, Dictionary<Guid, bool> Expanded) : IAction;
public record SetRunTemplateAction(Guid RunId, IRunTemplate Template) : IAction;

