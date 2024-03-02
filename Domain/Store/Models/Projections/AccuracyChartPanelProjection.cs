using Plotly.Blazor;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record AccuracyChartPanelProjection
{
    public Guid RunId { get; init; }
    public string TrainTestType { get; init; } = "";
    public List<ITrace> Traces { get; init; } = new();
}