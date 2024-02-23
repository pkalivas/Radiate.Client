using Plotly.Blazor;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record AccuracyChartPanelProjection
{
    public List<ITrace> Traces { get; init; } = new();
}