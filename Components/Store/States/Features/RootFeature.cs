using Plotly.Blazor;

namespace Radiate.Client.Components.Store.States.Features;

public record RootFeature : Feature<RootFeature>
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public RouteFeature? Route { get; init; } = new();
    public UiState UiState { get; init; } = new();
    public Dictionary<Guid, RunState> Runs { get; init; } = new();
    
    public override RootFeature Copy()
    {
        return new()
        {
            CurrentRunId = CurrentRunId,
            Route = Route?.Copy(),
            Runs = Runs.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Copy())
        };
    }
}

public record RouteFeature
{
    public string? Route { get; init; } = string.Empty;
    public Guid RunId { get; init; } = Guid.NewGuid();
}
