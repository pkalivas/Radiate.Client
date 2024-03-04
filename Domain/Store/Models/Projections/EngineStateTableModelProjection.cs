using Radiate.Client.Domain.Store.Models.States.Outputs;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record EngineStateTableModelProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public List<EngineOutput> EngineOutputs { get; init; } = new();
}