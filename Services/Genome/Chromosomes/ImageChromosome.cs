using System.Collections;
using Radiate.Client.Domain.Store.Models;
using Radiate.Client.Services.Genome.Genes;
using Radiate.Client.Services.Genome.Interfaces;
using Radiate.Optimizers.Evolution.Genome;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.Optimizers.Interfaces;
using Radiate.States;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace Radiate.Client.Services.Genome.Chromosomes;

public class ImageChromosome<TGene, TAllele> : IChromosome<TGene> 
    where TGene : ImageGene<TGene, TAllele>
    where TAllele : IDrawable, IFactory<TAllele>
{
    private readonly TGene[] _genes;
    
    public ImageChromosome(TGene[] genes)
    {
        _genes = genes;
    }
    
    public int Length => _genes.Length;
    
    public bool IsValid() => true;
    
    public ChromosomeState<TGene> Save() => new()
    {
        Genes = _genes
    };

    public IChromosome<TGene> Copy() => new ImageChromosome<TGene, TAllele>(_genes
        .Select(gene => gene.Copy())
        .ToArray());
    
    public IChromosome<TGene> NewInstance() => new ImageChromosome<TGene, TAllele>(_genes
        .Select(gene => gene.NewInstance())
        .ToArray());

    public TGene GetGene(int index = 0) => _genes[index];

    public void SetGene(int index, TGene newGene) => _genes[index] = newGene;

    public T As<T>() => (T) Convert.ChangeType(this, typeof(T));
    
    public IEnumerator<TGene> GetEnumerator() => ((IEnumerable<TGene>)_genes).GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    
    public ImageEntity Draw(int imageWidth, int imageHeight)
    {
        var format = "image/argb";
        
        using var image = new Image<Rgba32>(imageWidth, imageHeight);
        image.Mutate(x => x.Fill(Color.White));
        foreach (var circle in this.Select(gene => gene.Allele))
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