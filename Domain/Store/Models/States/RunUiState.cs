using System.Collections.Immutable;
using Radiate.Client.Domain.Templates;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunUiState
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public IRunTemplate? RunTemplate { get; init; }
    public string TrainTest { get; init; } = TrainTestTypes.Train;
    public IImmutableDictionary<Guid, PanelState> PanelStates { get; init; } = new Dictionary<Guid, PanelState>().ToImmutableDictionary();
}

public record PanelState : ITreeItem<Guid>
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public Guid Index { get; init; } = Guid.NewGuid();
    public IEnumerable<Guid> Children { get; init; } = new List<Guid>();
    public IPanel Panel { get; init; } = default!;
    public bool IsVisible { get; init; } = false;
    public bool IsExpanded { get; init; } = false;

    public string Key => $"{RunId}_{Index}_{IsVisible}_{IsExpanded}";
    
    public bool IsCyclic() => false;
}
