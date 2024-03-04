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
    public IImmutableDictionary<Guid, RunPanelState> Panels { get; init; } = new Dictionary<Guid, RunPanelState>().ToImmutableDictionary();
}

public record RunPanelState : ITreeItem
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public int Index { get; init; } = 0;
    public string PanelKey { get; init; } = "";
    public bool Visible { get; init; } = true;
    public IPanel Panel { get; init; } = default!;
    public IEnumerable<int> Children { get; init; } = new List<int>();
    public bool IsCyclic() => false;
}