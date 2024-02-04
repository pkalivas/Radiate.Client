using Reflow.Interfaces;

namespace Radiate.Client.Services.Store.Actions;

public record LayoutChangedAction : IAction
{
    public bool IsSidebarOpen { get; init; }
}

public record SetSelectedMetricsAction(Guid RunId, List<string> Metrics) : IAction;