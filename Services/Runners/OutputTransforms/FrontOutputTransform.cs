using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Optimizers.Evolution.Genome.Genes;
using Radiate.Optimizers.Evolution.MOEA;

namespace Radiate.Client.Services.Runners.OutputTransforms;

public class FrontOutputTransform : IRunOutputTransform<GeneticEpoch<FloatGene>, float[]>
{
    private Front<float[]>? _front;
    
    public RunOutputsState Transform(Guid runId, EngineOutput<GeneticEpoch<FloatGene>, float[]> handle, RunOutputsState output, RunInputsState input, bool isLast)
    {
        if (_front == null)
        {
            _front = new Front<float[]>(
                (one, two) => one.SequenceEqual(two),
                (one, two) => Pareto.Dominance(one, two, handle.Optimize));
        }

        _front.AddAll(handle.Epoch.Population
            .Select(val => val.GetFitnessValues()));
        
        if (_front.Size() < input.MultiObjectiveInputs.FrontMaxSize)
        {
            return  output with 
            {
                ParetoFrontOutput = new ParetoFrontOutput
                {
                    Front = _front.GetPoints().ToImmutableList()
                }
            };
        }

        if (_front.Size() >= input.MultiObjectiveInputs.FrontMaxSize)
        {
            _front = _front.ReplaceAll(points => Pareto.CrowdingDistance(points
                    .Select(val => val)
                    .ToArray(), handle.Optimize)
                .Select((val, idx) => (val, idx))
                .OrderByDescending(val => val.val)
                .Take(input.MultiObjectiveInputs.FrontMinSize)
                .Select(val => points[val.idx])
                .ToList());
        }

        return  output with 
        {
            ParetoFrontOutput = new ParetoFrontOutput
            {
                Front = _front.GetPoints().ToImmutableList()
            }
        };
    }
}