namespace Radiate.Client.Components.Store.Models;

public record ImageModel
{
    public int ImageWidth { get; set; } = 50;
    public int ImageHeight { get; set; } = 50;
    public ImageEntity Target { get; set; } = new();
    public ImageEntity Current { get; set; } = new();
}
