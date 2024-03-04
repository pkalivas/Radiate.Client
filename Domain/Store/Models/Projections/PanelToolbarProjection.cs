namespace Radiate.Client.Domain.Store.Models.Projections;

public record PanelToolbarProjection
{
    public Guid RunId { get; init; }
    public string ModelType { get; init; }
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsComplete { get; init; }
};