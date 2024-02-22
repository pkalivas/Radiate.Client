using System.Collections.Immutable;
using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.States;

public record UiState
{
    public bool IsSidebarOpen { get; init; } = true;
    // public IImmutableDictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new Dictionary<Guid, Dictionary<string, bool>>().ToImmutableDictionary();
    // public IImmutableDictionary<Guid, IRunTemplate> RunTemplates { get; init; } = new Dictionary<Guid, IRunTemplate>().ToImmutableDictionary();

    public override int GetHashCode() => HashCode.Combine(IsSidebarOpen);
}