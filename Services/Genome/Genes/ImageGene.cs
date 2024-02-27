using Radiate.Client.Services.Genome.Interfaces;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.Optimizers.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Genome.Genes;

public abstract class ImageGene<TGene, TAllele> : IGene<TGene, TAllele>, IDrawable
    where TGene : ImageGene<TGene, TAllele>
    where TAllele : IDrawable, IFactory<TAllele>
{
    protected readonly TAllele _allele;
    
    protected ImageGene(TAllele allele)
    {
        _allele = allele;
    }
    
    public TAllele Allele => _allele;
    
    public abstract void Draw(Image<Rgba32> image, int imageWidth, int imageHeight);
    public abstract TGene NewInstance(TAllele allele);
    
    public TGene Copy() => NewInstance(_allele.Copy());

    public TGene NewInstance() => NewInstance(_allele.NewInstance());

    public TGene NewInstance(TGene gene) => NewInstance(gene.Allele.Copy());

    public bool IsValid() => true;
}