using Radiate.Engines.Entities;

namespace Radiate.Client.Components.Store.Models;

public record MetricsModel
{
    public float Score => Scores?.Statistics?.LastValue ?? 0;
    public int Index { get; init; }
    public List<float> ScoresList { get; init; } = new();
    public MetricValue Scores { get; init; } = new();
    public MetricValue Fitness { get; init; } = new();
    public MetricValue GenomeSize { get; init; } = new();
    public MetricValue PopulationAge { get; init; } = new();
}
