namespace Radiate.Client.Services.Store.Models;

public record RunControlPanelModel
{
    public Guid RunId { get; init; }
    public bool IsStarted { get; init; }
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public double Score { get; init; }
    public TimeSpan ElapsedTime { get; init; }
    public int Index { get; init; }
    public RunInputsModel Inputs { get; init; } = new();
}
