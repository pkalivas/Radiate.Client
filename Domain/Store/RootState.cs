using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Domain.Store;

public record RootState : IState
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public UiState UiState { get; init; } = new();
    public Dictionary<Guid, RunState> Runs { get; init; } = new();
}
