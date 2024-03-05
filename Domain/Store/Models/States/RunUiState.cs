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
    public int Index { get; init; } = 0;
    public IPanel Panel { get; init; } = default!;

    public Guid Key => Panel.Id;
    public IEnumerable<Guid> Children => Panel.ChildPanels.Select(child => child.Id);
    public bool IsVisible => Panel is not GridPanel.GridItem gridItem || gridItem.IsVisible;
    public bool IsExpanded => Panel is not AccordionPanelItem item || item.Expanded;
    public string TrackByKey => $"{RunId}_{Key}_{IsVisible}_{IsExpanded}";
    public bool IsCyclic() => false;
}
