using Radiate.Client.Services.Genome.Interfaces;
using Radiate.Optimizers.Interfaces;
using Radiate.Schema;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Radiate.Client.Services.Genome;

public class Polygon : IMean<Polygon>, IFactory<Polygon>, IDrawable
{
    private readonly float[] _data;
    private readonly int _length;
    
    public Polygon(int length)
    {
        _length = length;
        _data = new float[4 + 2 * length];
    }
    
    public Polygon(int length, float[] data)
    {
        _length = length;
        _data = data;
    }
    
    public Polygon Mean(Polygon other)
    {
        var result = NewRandom(_length);
        for (var i = 0; i < _length; i++)
        {
            result._data[i] = (_data[i] + other._data[i]) * .5f;
        }
        
        return result;
    }
    
    public Polygon Copy() => new(_length, _data
        .Select(val => val)
        .ToArray());

    public Polygon Mutate(float rate, float magnitude)
    {
        var random = RandomRegistry.GetRandom();
        var mutated = new Polygon(_length);
        
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

    public void Draw(Image<Rgba32> image, int width, int height)
    {
        var color = new Rgba32(_data[0], _data[1], _data[2], _data[3]);

        var pathBuilder = new PathBuilder();

        var startPoint = new PointF(_data[4] * width, _data[5] * height);
        pathBuilder.StartFigure();

        for (int j = 1; j < _length; ++j)
        {
            var point = new PointF(_data[4 + j * 2] * width, _data[5 + j * 2] * height);
            pathBuilder.AddLine(startPoint, point);
            startPoint = point;
        }

        pathBuilder.CloseFigure();

        var path = pathBuilder.Build();
        image.Mutate(x => x.Fill(color, path));
    }

    public static Polygon NewRandom(int length, Random? random = null)
    {
        random ??= RandomRegistry.GetRandom();
        var result = new Polygon(length);
        
        result._data[0] = (float)random.NextDouble(); // r
        result._data[1] = (float)random.NextDouble(); // g
        result._data[2] = (float)random.NextDouble(); // b
        result._data[3] = Math.Max(0.2f, (float)(random.NextDouble() * random.NextDouble())); // a
        
        // var px = 0.5f;
        // var py = 0.5f;
        var px = (float)(random.NextDouble() * 2 - 1);
        var py = (float)(random.NextDouble() * 2 - 1);
        
        for (var k = 0; k < length; k++)
        {
            px += (float) (random.NextDouble() - 0.5f);// * 0.5f;
            py += (float) (random.NextDouble() - 0.5f);// * 0.5f;
            
            result._data[4 + 2 * k] = px = Clamp(px);
            result._data[5 + 2 * k] = py = Clamp(py);            
        }
        
        return result;
    }
    
    private static float Clamp(float val) => val < 0f ? 0f : Math.Min(val, 1f);
    public Polygon NewInstance() => NewRandom(_length);
}