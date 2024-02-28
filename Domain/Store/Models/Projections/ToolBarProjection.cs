namespace Radiate.Client.Domain.Store.Models.Projections;

public record ToolBarProjection
{
    public Guid RunId { get; init; }
    public string ModelType { get; init; } = "";
    public string DataSetType { get; init; } = "";
}