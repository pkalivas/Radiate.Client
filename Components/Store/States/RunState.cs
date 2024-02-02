using Plotly.Blazor;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.Schema;

namespace Radiate.Client.Components.Store.States;

public record RunState : ICopy<RunState>
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public int Index { get; init; }
    public bool IsRunning { get; init; }
    public string Status { get; init; } = "";
    public RunInputState Inputs { get; init; } = new();
    public RunOutputsState Outputs { get; init; } = new();
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