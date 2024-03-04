using Radiate.Extensions.Operations;
using Radiate.Extensions.Schema;

namespace Radiate.Client.Domain.Store.Models;

public record NodeItem : ITreeItem
{
    public Guid NodeId { get; init; }
    public int Index { get; init; }
    public IOp<float> Op { get; init; }
    public NodeTypes NodeType { get; init; }
    public DirectionTypes Direction { get; init; }
    public bool IsValid { get; init; }
    public bool IsEnabled { get; init; }
    public bool IsRecurrent { get; init; }
    public HashSet<int> Incoming { get; init; } = new();
    public HashSet<int> Outgoing { get; init; } = new();
    public IEnumerable<int> Children { get; init; } = [];
    public bool IsCyclic() => IsRecurrent;
}
