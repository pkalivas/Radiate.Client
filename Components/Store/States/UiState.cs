using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.States;

public class UiState : ICopy<UiState>
{
    public Dictionary<Guid, Dictionary<string, bool>> EngineStateExpanded { get; init; } = new();
    
    public UiState Copy()
    {
        throw new NotImplementedException();
    }
}