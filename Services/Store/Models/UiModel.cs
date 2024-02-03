namespace Radiate.Client.Services.Store.Models;

public record UiModel
{
    public bool IsSidebarOpen { get; init; } = true;
    public Dictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new();
}