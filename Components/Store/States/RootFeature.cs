namespace Radiate.Client.Components.Store.States;

public record RootFeature : Feature<RootFeature>
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public RouteFeature? Route { get; init; } = new();
    public Dictionary<Guid, RunFeature> Runs { get; init; } = new();
}

public record RouteFeature : Feature<RouteFeature>
{
    public string? Route { get; init; } = string.Empty;
    public Guid RunId { get; init; } = Guid.NewGuid();
}

public record RunFeature : Feature<RunFeature>
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public string Status { get; init; } = "";
    public EngineInputsState Inputs { get; init; } = new();
    public EngineOutputState Outputs { get; init; } = new();
}