using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store;

public record RootState : IState
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public RouteFeature? Route { get; init; } = new();
    public UiModel UiModel { get; init; } = new();
    public Dictionary<Guid, RunModel> Runs { get; init; } = new();
}

public record RouteFeature
{
    public string? Route { get; init; } = string.Empty;
    public Guid RunId { get; init; } = Guid.NewGuid();
}
