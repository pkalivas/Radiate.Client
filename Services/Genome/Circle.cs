using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.Optimizers.Interfaces;
using Radiate.Schema;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Radiate.Client.Services.Genome;

public class Circle : IMean<Circle>, ICopy<Circle>
{
    private readonly float[] _data;
    
    public Circle(float[] data)
    {
        _data = data;
    }
    
    public Circle Copy() => new(_data
        .Select(val => val)
        .ToArray());

    public Circle Mean(Circle other)
    {
        var result = NewRandom();
        for (var i = 0; i < _data.Length; i++)
        {
            result._data[i] = (_data[i] + other._data[i]) * .5f;
        }
        
        return result;
    }
    
    public Circle Mutate(float rate, float magnitude)
    {
        var random = RandomRegistry.GetRandom();
        var mutated = NewRandom(random);
        
        for (var i = 0; i < _data.Length; i++)
        {
            if (random.NextDouble() < rate)
            {
                mutated._data[i] = Clamp(_data[i] + (float)(random.NextDouble() * 2 - 1) * magnitude);
            }
            else
            {
                mutated._data[i] = _data[i];
            }
        }
        
        return mutated;
    }
    
    public void Draw(Image<Rgba32> image, int imageWidth, int imageHeight)
    {
        var color = new Rgba32(_data[0], _data[1], _data[2], _data[3]);
        
        var circle = new EllipsePolygon(
            _data[4] * imageWidth,
            _data[5] * imageHeight,
            Math.Max(_data[6] * (imageWidth / 10f), 1e-10f));
        
        image.Mutate(ctx => ctx.Fill(color, circle));
    }
    
    public static Circle NewRandom(Random? random = null)
    {
        random ??= RandomRegistry.GetRandom();
        var data = new float[7];
        
        for (var i = 0; i < 7; i++)
        {
            if (i != 3)
            {
                data[i] = (float)random.NextDouble();
            }
            else
            {
                data[i] = Math.Max(0.25f, (float)(random.NextDouble() * random.NextDouble())); // a
            }
        }
        
        return new Circle(data);
    }
    
    private static float Clamp(float val) => val < 0f ? 0f : Math.Min(val, 1f);
}