using System.Collections.Immutable;
using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.States;

public record RunUiState
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public IRunTemplate? RunTemplate { get; init; }
    public IImmutableDictionary<string, bool> EngineStateExpanded { get; init; } = 
        new Dictionary<string, bool>().ToImmutableDictionary();
    public IImmutableDictionary<Guid, bool> PanelExpanded { get; init; } = 
        new Dictionary<Guid, bool>().ToImmutableDictionary();
}