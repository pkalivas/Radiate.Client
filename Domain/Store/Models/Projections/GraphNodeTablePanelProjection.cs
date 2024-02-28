using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record GraphNodeTablePanelProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public PerceptronGraph<float>? Graph { get; init; } = new();
}