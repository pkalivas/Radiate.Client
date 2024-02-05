using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Store;

public record RootState : IState
{
    public Guid CurrentRunId { get; init; } = Guid.NewGuid();
    public UiModel UiModel { get; init; } = new();
    public Dictionary<Guid, RunModel> Runs { get; init; } = new();
}
