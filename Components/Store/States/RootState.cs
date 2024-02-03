using Radiate.Client.Components.Store.Models;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.States;

public record RootState
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public RouteFeature? Route { get; init; } = new();
    public UiFeature UiFeature { get; init; } = new();
    public Dictionary<Guid, RunModel> Runs { get; init; } = new();
    public Dictionary<Guid, ImageModel> Images { get; set; } = new();
}

public record RouteFeature
{
    public string? Route { get; init; } = string.Empty;
    public Guid RunId { get; init; } = Guid.NewGuid();
}
