using Radiate.Client.Domain.Store.Models.States.Outputs;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record EngineStateTableModelProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public bool IsComplete { get; init; } = false;
    public EngineOutput[] EngineOutputs { get; init; } = Array.Empty<EngineOutput>();
}