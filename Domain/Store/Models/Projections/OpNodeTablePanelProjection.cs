namespace Radiate.Client.Domain.Store.Models.Projections;

public record OpNodeTablePanelProjection
{
    public Guid RunId { get; init; } = Guid.Empty;
    public bool IsComplete { get; init; } = false;
    // public NodeGroup<IOp<float>> NodeGroup { get; set; } = new();
    public NodeItem[] NodeItems { get; init; } = Array.Empty<NodeItem>();
}