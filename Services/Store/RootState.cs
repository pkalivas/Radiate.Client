using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.States;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store;

public record RootState : IState
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public UiState UiState { get; init; } = new();
    public Dictionary<Guid, RunState> Runs { get; init; } = new();
}
