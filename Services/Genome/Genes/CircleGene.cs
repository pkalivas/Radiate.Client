using Radiate.Optimizers.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Genome.Genes;

public class CircleGene : ImageGene<CircleGene, Circle>, IMean<CircleGene>
{
    public CircleGene(Circle allele) : base(allele) { }

    public override void Draw(Image<Rgba32> image, int imageWidth, int imageHeight) => 
        Allele.Draw(image, imageWidth, imageHeight);

    public override CircleGene NewInstance(Circle allele) => new(allele);
    public CircleGene Mean(CircleGene other) => new(Allele.Mean(other.Allele));
}
