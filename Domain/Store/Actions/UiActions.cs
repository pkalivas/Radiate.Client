using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store.Actions;

public record LayoutChangedAction : IAction
{
    public bool IsSidebarOpen { get; init; }
}

public record NavigateToRunAction(Guid RunId) : IAction;

public record SetRunLoadingAction(Guid RunId, bool Loading) : IAction;

public record SetTrainTestTypeAction(Guid RunId, string TrainTestType) : IAction;

