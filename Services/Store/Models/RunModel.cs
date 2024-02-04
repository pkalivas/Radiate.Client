using Radiate.Schema;

namespace Radiate.Client.Services.Store.Models;

public record RunModel
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public int Index { get; init; }
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsCompleted { get; init; }
    public string Status { get; init; } = "";
    public RunInputsModel Inputs { get; init; } = new();
    public RunOutputsModel Outputs { get; init; } = new();
    public Dictionary<string, MetricValueModel> Metrics { get; init; } = new();
    public HashSet<string> SelectedMetrics { get; init; } = new();
    public List<float> Scores { get; init; } = new();
    
    public override int GetHashCode()
    {
        return Hash.Of(typeof(RunModel))
            .And(RunId)
            .And(IsRunning)
            .And(Status)
            .And(Inputs)
            .And(Outputs)
            .And(Scores)
            .Value;
    }
}