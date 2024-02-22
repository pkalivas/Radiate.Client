using System.Collections.Immutable;
using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunUiState
{
    public Guid RunId { get; set; } = Guid.NewGuid();
    public IRunTemplate? RunTemplate { get; set; } = null;
    public IImmutableDictionary<string, bool> EngineStateExpanded { get; set; } = new Dictionary<string, bool>().ToImmutableDictionary();
    public IImmutableDictionary<Guid, bool> PanelExpanded { get; set; } = new Dictionary<Guid, bool>().ToImmutableDictionary();
}