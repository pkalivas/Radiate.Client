namespace Radiate.Client.Components.Store.States.Features;

public record UiFeature
{
    public bool IsSidebarOpen { get; init; } = true;
    public Dictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new();
}