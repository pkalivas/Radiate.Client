using Radiate.Optimizers.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Genome.Genes;

public class PolygonGene : ImageGene<PolygonGene, Polygon>, IMean<PolygonGene>
{
    public PolygonGene(Polygon polygon) : base(polygon) { }
    
    public PolygonGene Mean(PolygonGene other) => new(Allele.Mean(other.Allele));
    public override void Draw(Image<Rgba32> image, int imageWidth, int imageHeight) =>
        Allele.Draw(image, imageWidth, imageHeight);
    
    public override PolygonGene NewInstance(Polygon allele) => new(allele);
}