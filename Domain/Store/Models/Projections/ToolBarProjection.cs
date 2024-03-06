namespace Radiate.Client.Domain.Store.Models.Projections;

public record ToolBarProjection
{
    public Guid RunId { get; init; }
    public string ModelType { get; init; } = "";
    public string DataSetType { get; init; } = "";
    public bool IsRunning { get; init; } = false;
    public bool IsComplete { get; init; } = false;
}