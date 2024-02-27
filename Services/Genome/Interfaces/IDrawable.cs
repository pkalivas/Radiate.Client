using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Genome.Interfaces;

public interface IDrawable
{
    void Draw(Image<Rgba32> image, int imageWidth, int imageHeight);
}