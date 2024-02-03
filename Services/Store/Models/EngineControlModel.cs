namespace Radiate.Client.Services.Store.Models;

public record EngineControlModel
{
    public Guid RunId { get; init; }
    public bool IsStarted { get; init; }
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsCompleted { get; init; }
}
