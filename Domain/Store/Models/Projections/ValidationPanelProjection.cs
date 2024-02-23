using Plotly.Blazor;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record ValidationPanelProjection
{
    public Guid RunId { get; init; }
    public string LossFunction { get; init; } = "";
    public List<ValidationSet> Validations { get; init; } = new();
    public List<ITrace> Traces { get; init; } = new();
}

public record ValidationSet
{
    public string Name { get; init; } = "";
    public float ClassificationAccuracy { get; init; }
    public float RegressionAccuracy { get; init; }
    public float CategoricalAccuracy { get; init; }
    public float TotalLoss { get; init; }
    public TimeSpan Duration { get; init; }
    public int DataPoints { get; init; }
}
