namespace Radiate.Client.Domain.Store.Models.Projections;

public record StandardRunUiProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public List<Guid> PanelIds { get; init; } = new();
    public bool IsLoading { get; init; } = true;
}