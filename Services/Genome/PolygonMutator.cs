using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Services.Genome;

public class PolygonMutator : Mutator<PolygonGene>
{
    private readonly float _rate;
    private readonly float _magnitude;
    
    public PolygonMutator(float rate, float magnitude)
    {
        _rate = rate;
        _magnitude = magnitude;
    }
    
    public override int Mutate(IChromosome<PolygonGene> chromosome)
    {
        for (var i = 0; i < chromosome.Length; i++)
        {
            var gene = chromosome.GetGene(i);
            chromosome.SetGene(i, gene.NewInstance(gene.Allele.Mutate(_rate, _magnitude)));
        }
        
        return chromosome.Length;
    }
}