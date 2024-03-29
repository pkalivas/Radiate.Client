using System.Collections.Immutable;
using Radiate.Schema;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunState
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public int Index { get; init; }
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public RunInputsState Inputs { get; init; } = new();
    public RunOutputsState Outputs { get; init; } = new();
    public IImmutableList<float> Scores { get; init; } = ImmutableList<float>.Empty;
    
    public override int GetHashCode()
    {
        return Hash.Of(typeof(RunState))
            .And(RunId)
            .And(IsRunning)
            .And(IsPaused)
            .And(IsCompleted)
            .And(StartTime)
            .And(EndTime)
            .And(Index)
            .And(Inputs)
            .And(Outputs)
            .And(Scores)
            .Value;
    }
}