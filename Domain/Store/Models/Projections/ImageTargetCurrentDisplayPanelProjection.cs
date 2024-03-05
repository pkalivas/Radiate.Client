namespace Radiate.Client.Domain.Store.Models.Projections;

public record ImageTargetCurrentDisplayPanelProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public bool IsComplete { get; set; } = false;
    public bool IsRunning { get; set; } = false;
    public ImageEntity TargetImage { get; set; } = new();
    public ImageEntity CurrentImage { get; set; } = new();
}

public record ImageDisplayPanelProjection
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public string ImageType { get; init; } = "";
    public bool IsComplete { get; set; } = false;
    public bool IsRunning { get; set; } = false;
    public int Width { get; set; } = 0;
    public int Height { get; set; } = 0;
    public int DisplayWidth { get; init; } = 500;
    public int DisplayHeight { get; init; } = 500;
    public ImageEntity Image { get; set; } = new();
}