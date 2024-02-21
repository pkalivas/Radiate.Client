namespace Radiate.Client.Domain.Store.Models.Projections;

public record MetricSummaryChartPanelProjection
{
    public Guid RunId { get; init; }
    public string MetricName { get; init; } = string.Empty;
    public MetricValueModel Value { get; init; } = new();
}