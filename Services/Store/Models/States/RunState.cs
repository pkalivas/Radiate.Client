using Radiate.Client.Services.Store.Models.Projections;
using Radiate.Schema;

namespace Radiate.Client.Services.Store.Models.States;

public record RunState
{
    public Guid RunId { get; } = Guid.NewGuid();
    public int Index { get; init; }
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public RunInputsState Inputs { get; init; } = new();
    public RunOutputsState Outputs { get; init; } = new();
    public Dictionary<string, MetricValueModel> Metrics { get; init; } = new();
    public List<float> Scores { get; init; } = new();
    
    public override int GetHashCode()
    {
        return Hash.Of(typeof(RunState))
            .And(RunId)
            .And(IsRunning)
            .And(Inputs)
            .And(Outputs)
            .And(Scores)
            .Value;
    }
}