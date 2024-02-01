
using Plotly.Blazor;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.Schema;

namespace Radiate.Client.Components.Store.States.Features;

public record RootFeature : Feature<RootFeature>
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public RouteFeature? Route { get; init; } = new();
    public Dictionary<Guid, RunState> Runs { get; init; } = new();

    public override int GetHashCode()
    {
        return Hash.Of(typeof(RouteFeature))
            .And(Route)
            .And(Runs)
            .And(Name)
            .Value;
    }
    
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

public record RunState : ICopy<RunState>
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsRunning { get; init; }
    public string Status { get; init; } = "";
    public EngineInputsState Inputs { get; init; } = new();
    public EngineOutputState Outputs { get; init; } = new();
    public List<float> Scores { get; init; } = new();

    public RunState Copy()
    {
        return new()
        {
            RunId = RunId,
            IsRunning = IsRunning,
            Status = Status,
            Inputs = Inputs.Copy(),
            Outputs = Outputs.Copy(),
            Scores = Scores.ToList()
        };
    }

    public override int GetHashCode()
    {
        return Hash.Of(typeof(RunState))
            .And(RunId)
            .And(IsRunning)
            .And(Status)
            .And(Inputs)
            .And(Outputs)
            .And(Scores)
            .Value;
    }
}