using Radiate.Client.Domain.Templates;

namespace Radiate.Client.Domain.Store.Models.States;

public record UiState
{
    public bool IsSidebarOpen { get; init; } = true;
    public Dictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new();
    public Dictionary<Guid, IRunTemplate> RunTemplates { get; init; } = new();
}