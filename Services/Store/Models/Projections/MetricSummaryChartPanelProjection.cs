namespace Radiate.Client.Services.Store.Models.Projections;

public record MetricSummaryChartPanelProjection
{
    public Guid RunId { get; init; }
    public string MetricName { get; init; } = string.Empty;
    public MetricValueModel Value { get; init; } = new();
}