using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Services.Genome;

public class PolygonGene : IGene<PolygonGene, Polygon>, IMean<PolygonGene>
{
    private readonly Polygon _polygon;
    
    public PolygonGene(Polygon polygon)
    {
        _polygon = polygon;
    }
    
    public Polygon Allele => _polygon;
    
    public bool IsValid() => true;

    public PolygonGene Copy() => new(_polygon.Copy());

    public PolygonGene NewInstance() => new(Polygon.NewRandom(_polygon.Length));

    public PolygonGene NewInstance(PolygonGene gene) => new(gene.Allele.Copy());
    
    public PolygonGene NewInstance(Polygon allele) => new(allele.Copy());
    
    public PolygonGene Mean(PolygonGene other) => new(_polygon.Mean(other.Allele));
}