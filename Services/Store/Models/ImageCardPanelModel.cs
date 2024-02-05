namespace Radiate.Client.Services.Store.Models;

public record ImageCardPanelModel
{
    public Guid RunId { get; init; } = Guid.NewGuid();
    public int Height { get; set; } = 50;
    public int Width { get; set; } = 50;
    public ImageEntity TargetImage { get; set; } = new();
    public ImageEntity CurrentImage { get; set; } = new();
}