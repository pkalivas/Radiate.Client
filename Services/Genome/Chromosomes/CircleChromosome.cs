using System.Collections;
using Radiate.Client.Domain.Store.Models;
using Radiate.Client.Services.Genome.Genes;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Radiate.Client.Services.Genome.Chromosomes;

public class CircleChromosome : IChromosome<CircleGene>
{
    private readonly CircleGene[] _genes;
    
    public CircleChromosome(CircleGene[] genes)
    {
        _genes = genes;
    }

    public int Length => _genes.Length;
    
    public IEnumerator<CircleGene> GetEnumerator() => ((IEnumerable<CircleGene>) _genes).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public ChromosomeState<CircleGene> Save() => new(_genes.Select(gene => gene).ToArray());

    public IChromosome<CircleGene> Copy() => 
        new CircleChromosome(_genes.Select(gene => gene.Copy()).ToArray());

    public IChromosome<CircleGene> NewInstance() => 
        new CircleChromosome(_genes.Select(gene => gene.NewInstance()).ToArray());

    public bool IsValid() => true;

    public CircleGene GetGene(int index = 0) => _genes[index];

    public void SetGene(int index, CircleGene newGene) => _genes[index] = newGene;

    public T As<T>() => (T) Convert.ChangeType(this, typeof(T));
    
    public ImageEntity Draw(int imageWidth, int imageHeight)
    {
        var format = "image/argb";
        
        using var image = new Image<Rgba32>(imageWidth, imageHeight);
        image.Mutate(x => x.Fill(Color.White));
        foreach (var circle in _genes.Select(gene => gene.Allele))
        {
            circle.Draw(image, imageWidth, imageHeight);
        }
        
        image.Mutate(x => x.Resize(imageWidth, imageHeight));
        
        using var imageStream = new MemoryStream();
        image.SaveAsPng(imageStream);

        return new ImageEntity
        {
            ImageData = image.Clone(),
        };
    }
}