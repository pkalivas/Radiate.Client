using Radiate.Client.Domain.Store.Models.States;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record RunListPanelProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public List<RunState> Runs { get; init; }
}