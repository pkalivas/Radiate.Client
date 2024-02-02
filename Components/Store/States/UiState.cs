using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.States;

public record UiState : ICopy<UiState>
{
    public bool IsSidebarOpen { get; init; } = true;
    public Dictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new();
    
    public UiState Copy() => new()
    {
        EngineStateExpanded = EngineStateExpanded.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)),
        IsSidebarOpen = IsSidebarOpen
    };
}