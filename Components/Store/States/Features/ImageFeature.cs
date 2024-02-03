using Radiate.Client.Components.Store.Models;

namespace Radiate.Client.Components.Store.States.Features;

public record ImageFeature
{
    public int ImageWidth { get; set; } = 50;
    public int ImageHeight { get; set; } = 50;
    public ImageEntity Target { get; set; } = new();
    public ImageEntity Current { get; set; } = new();
}
