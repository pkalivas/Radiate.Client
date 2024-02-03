namespace Radiate.Client.Services.Store.Models;

public record ImageModel
{
    public int ImageWidth { get; set; } = 50;
    public int ImageHeight { get; set; } = 50;
    public ImageEntity Image { get; set; } = new();
    public ImageEntity Target { get; set; } = new();
    public ImageEntity Current { get; set; } = new();
}
