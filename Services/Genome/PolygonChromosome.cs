using System.Collections;
using Radiate.Client.Services.Store.Models;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Radiate.Client.Services.Genome;

public class PolygonChromosome : IChromosome<PolygonGene>
{
    private readonly PolygonGene[] _genes;
    
    public PolygonChromosome(PolygonGene[] genes)
    {
        _genes = genes;
    }
    
    public int Length => _genes.Length;
    
    public bool IsValid() => true;
    
    public ChromosomeState<PolygonGene> Save() => new()
    {
        Genes = _genes
    };

    public IChromosome<PolygonGene> Copy() => new PolygonChromosome(_genes
        .Select(gene => gene.Copy())
        .ToArray());
    
    public IChromosome<PolygonGene> NewInstance() => new PolygonChromosome(_genes
        .Select(gene => gene.NewInstance())
        .ToArray());

    public PolygonGene GetGene(int index = 0) => _genes[index];

    public void SetGene(int index, PolygonGene newGene) => _genes[index] = newGene;

    public T As<T>() => (T) Convert.ChangeType(this, typeof(T));
    
    public IEnumerator<PolygonGene> GetEnumerator() => ((IEnumerable<PolygonGene>)_genes).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public ImageEntity Draw(int imageWidth, int imageHeight)
    {
        var format = "image/argb";

        using var image = new Image<Rgba32>(imageWidth, imageHeight);
        image.Mutate(x => x.Fill(Color.White));
        foreach (var polygon in _genes.Select(gene => gene.Allele))
        {
            polygon.Draw(image, imageWidth, imageHeight);
        }

        using var imageStream = new MemoryStream();
        image.SaveAsPng(imageStream);

        return new ImageEntity
        {
            ImageData = image.Clone(),
        };
    }
    
    
}