using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Store.Models;

public record ImageEntity
{
    public Image<Rgba32> ImageData { get; set; } = new(1, 1);
    public string ImageDisplay => $"data:image/argb;base64,{ImageDataString()}";
    public string ImageDataString()
    {
        using var stream = new MemoryStream();
        ImageData.SaveAsPng(stream);
        return Convert.ToBase64String(stream.ToArray());
    }
}
