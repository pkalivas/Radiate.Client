using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.States;

public class UiState : ICopy<UiState>
{
    public Dictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new();
    
    public UiState Copy() => new()
    {
        EngineStateExpanded = EngineStateExpanded.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToDictionary(kvp => kvp.Key, kvp => kvp.Value))
    };
}