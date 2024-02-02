using Radiate.Client.Components.Store.Models;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Components.Store.States;

public record ImageState : ICopy<ImageState>
{
    public int ImageWidth { get; set; } = 50;
    public int ImageHeight { get; set; } = 50;
    public ImageEntity Target { get; set; } = new();
    public ImageEntity Current { get; set; } = new();

    public ImageState Copy() => new()
    {
        ImageWidth = ImageWidth,
        ImageHeight = ImageHeight,
        Current = Current,
        Target = Target
    };
}
