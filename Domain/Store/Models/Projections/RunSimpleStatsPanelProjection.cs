namespace Radiate.Client.Domain.Store.Models.Projections;

public record RunSimpleStatsPanelProjection
{
    public Guid RunId { get; init; }
    public DateTime StartTime { get; init; }
    public DateTime EndTime { get; init; }
    public TimeSpan ElapsedTime { get; init; }
    public double Score { get; init; }
    public int Index { get; init; }
}