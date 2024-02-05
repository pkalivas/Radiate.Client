using Radiate.Client.Services.Store.Models.States;

namespace Radiate.Client.Services.Store.Models.Projections;

public record RunListPanelProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public List<RunState> Runs { get; init; }
}