using System.ComponentModel;
using System.Runtime.CompilerServices;
using Radiate.Engines.Schema;

namespace Radiate.Client.Services.Store.Models;

public record MetricListModel
{
    public Guid RunId { get; init; }
    public HashSet<MetricValueModel> SelectedMetrics { get; init; } = new();
    public List<MetricValueModel> Values { get; init; } = new();
}

public record MetricValueModel
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public MetricTypes MetricType { get; init; } = MetricTypes.Description;
    public double[] Distribution { get; init; } = Array.Empty<double>();
    public double Value { get; init; }
    public double Min { get; init; }
    public double Max { get; init; }
    public double Mean { get; init; }
    public double Variance { get; init; }
    public double Skewness { get; init; }
    public double Kurtosis { get; init; }
    public double Sum { get; init; }
    public TimeSpan Current { get; init; } = TimeSpan.Zero;
    public TimeSpan Total { get; init; } = TimeSpan.Zero;
    public TimeSpan MeanTime { get; init; } = TimeSpan.Zero;
    public TimeSpan MinTime { get; init; } = TimeSpan.Zero;
    public TimeSpan MaxTime { get; init; } = TimeSpan.Zero;
}