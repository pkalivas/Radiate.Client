using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Genome.Interfaces;
using Radiate.Schema;

namespace Radiate.Client.Services.Genome;

public class UniformCrossover<TGene> : Crossover<TGene> where TGene : IGene<TGene>
{
    private readonly float _crossoverRate;
    
    public UniformCrossover(float crossoverRate)
    {
        _crossoverRate = crossoverRate;
    }
    
    public override void Cross(IChromosome<TGene> child, IChromosome<TGene> parentTwo)
    {
        var random = RandomRegistry.GetRandom();
        
        for (var i = 0; i < child.Length; i++)
        {
            if (random.NextDouble() < _crossoverRate)
            {
                Swap(child, i, i + 1, parentTwo, i);
            }
        }
    }
    
    private static void Swap(IChromosome<TGene> one, int start, int end, IChromosome<TGene> two, int otherStart)
    {
        if (otherStart < 0 || otherStart + (end - start) > one.Length)
        {
            throw new IndexOutOfRangeException($"Invalid index range: [{otherStart}, {otherStart + (end - start)})");
        }

        if (start >= end)
        { 
            return;
        }
             
        for (var i = end - start; --i >= 0;)
        {
            var temp = one.GetGene(start + i);
            var otherGene = two.GetGene(otherStart + i);
            
            one.SetGene(start + i, temp.NewInstance(otherGene));
            two.SetGene(otherStart + i, otherGene.NewInstance(temp));
        }
    }
}

