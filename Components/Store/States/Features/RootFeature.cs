
namespace Radiate.Client.Components.Store.States.Features;

public record RootFeature : Feature<RootFeature>
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public RouteFeature? Route { get; init; } = new();
    public Dictionary<Guid, RunState> Runs { get; init; } = new();
}

public record RouteFeature : Feature<RouteFeature>
{
    public string? Route { get; init; } = string.Empty;
    public Guid RunId { get; init; } = Guid.NewGuid();
}

public record RunState
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public string Status { get; init; } = "";
    public EngineInputsState Inputs { get; init; } = new();
    public EngineOutputState Outputs { get; init; } = new();
}