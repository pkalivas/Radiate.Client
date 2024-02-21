namespace Radiate.Client.Domain.Store.Models.Projections;

public record PanelToolbarProjection
{
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsComplete { get; init; }
    public int Index { get; init; }
    public float Score { get; init; }
    public TimeSpan Duration { get; init; }
    public string EngineState { get; init; }
};