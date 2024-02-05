namespace Radiate.Client.Services.Store.Models.States;

public record UiState
{
    public bool IsSidebarOpen { get; init; } = true;
    public Dictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new();
}