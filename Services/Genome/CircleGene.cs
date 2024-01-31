using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Services.Genome;

public class CircleGene : IGene<CircleGene, Circle>, IMean<CircleGene>
{
    private readonly Circle _circle;
    
    public CircleGene(Circle circle)
    {
        _circle = circle;
    }
    
    public Circle Allele => _circle;

    public CircleGene Copy() => new(_circle.Copy());

    public CircleGene NewInstance() => new(Circle.NewRandom());

    public CircleGene NewInstance(CircleGene gene) => new(gene.Allele.Copy());

    public bool IsValid() => true;

    public CircleGene NewInstance(Circle allele) => new(allele.Copy());
    public CircleGene Mean(CircleGene other) => new(_circle.Mean(other.Allele));
}