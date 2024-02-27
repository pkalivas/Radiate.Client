using Radiate.Client.Services.Genome.Genes;
using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Genome.Interfaces;

namespace Radiate.Client.Services.Genome.Alterers;

public class CircleMutator : Mutator<CircleGene>
{
    private readonly float _rate;
    private readonly float _magnitude;
    
    public CircleMutator(float rate, float magnitude)
    {
        _rate = rate;
        _magnitude = magnitude;
    }
    
    public override int Mutate(IChromosome<CircleGene> chromosome)
    {
        for (var i = 0; i < chromosome.Length; i++)
        {
            var gene = chromosome.GetGene(i);
            chromosome.SetGene(i, gene.NewInstance(gene.Allele.Mutate(_rate, _magnitude)));
        }
        
        return chromosome.Length;
    }
}