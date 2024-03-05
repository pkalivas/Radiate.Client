namespace Radiate.Client.Domain.Store.Models.Projections;

public record ImageHeaderProjection
{
    public Guid RunId { get; init; }
    public string ImageType { get; init; } = "";
    public bool IsRunning { get; init; }
    public bool IsPaused { get; init; }
    public bool IsComplete { get; init; }
    public int DisplayWidth { get; init; }
    public int DisplayHeight { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }
    public ImageEntity Image { get; init; } = new();
};