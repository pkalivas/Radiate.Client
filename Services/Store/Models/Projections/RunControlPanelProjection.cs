using Radiate.Client.Services.Store.Models.States;

namespace Radiate.Client.Services.Store.Models.Projections;

public record RunControlPanelProjection
{
    public Guid RunId { get; init; }
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsCompleted { get; init; }
    public bool NeedsImageUpload { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public double Score { get; init; }
    public TimeSpan ElapsedTime { get; init; }
    public int Index { get; init; }
    public RunInputsState Inputs { get; init; } = new();
}
