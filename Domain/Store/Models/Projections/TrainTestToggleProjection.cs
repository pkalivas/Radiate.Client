using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record TrainTestToggleProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public string TrainTest { get; init; } = TrainTestTypes.Train;
}