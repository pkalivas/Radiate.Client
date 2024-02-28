using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Operations;

namespace Radiate.Client.Domain.Store.Models.Projections;

public record OpNodeTablePanelProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public bool IsComplete { get; init; } = false;
    public NodeGroup<IOp<float>> NodeGroup { get; set; } = new();
}