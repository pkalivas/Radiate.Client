namespace Radiate.Client.Services.Store.Models.Projections;

public record ImageTargetCurrentDisplayPanelProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsComplete { get; set; } = false;
    public bool IsRunning { get; set; } = false;
    public ImageEntity TargetImage { get; set; } = new();
    public ImageEntity CurrentImage { get; set; } = new();
}