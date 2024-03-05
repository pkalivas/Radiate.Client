namespace Radiate.Client.Domain.Store.Models.Projections;

public record ModelDownloadProjection
{
    public Guid RunId { get; init; }
    public bool IsRunning { get; init; } = false;
    public string JsonData { get; init; } = "";
}