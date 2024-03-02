using System.Collections.Immutable;
using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Optimizers.Evolution.Genome.Genes;
using Radiate.Optimizers.Evolution.MOEA;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class MultiObjectiveRunner : EngineRunner<GeneticEpoch<FloatGene>, float[]>
{
    private Front<float[]>? _front;
    
    protected MultiObjectiveRunner(IStore<RootState> store) : base(store) { }

    protected override RunOutputsState MapOnOutput(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<FloatGene>, float[]> output) => AddToFront(runInputs, runOutputs, output);

    protected override RunOutputsState MapOnStop(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<FloatGene>, float[]> output) => AddToFront(runInputs, runOutputs, output);

    private RunOutputsState AddToFront(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<FloatGene>, float[]> output)
    {
        if (_front == null)
        {
            _front = new Front<float[]>(
                (one, two) => one.SequenceEqual(two),
                (one, two) => Pareto.Dominance(one, two, output.Optimize));
        }

        _front.AddAll(output.Epoch.Population
            .Select(val => val.GetFitnessValues()));
        
        if (_front.Size() < runInputs.MultiObjectiveInputs.FrontMaxSize)
        {
            return runOutputs with 
            {
                ParetoFrontOutput = new ParetoFrontOutput
                {
                    Front = _front.GetPoints().ToImmutableList()
                }
            };
        }

        if (_front.Size() >= runInputs.MultiObjectiveInputs.FrontMaxSize)
        {
            _front = _front.ReplaceAll(points => Pareto.CrowdingDistance(points
                    .Select(val => val)
                    .ToArray(), output.Optimize)
                .Select((val, idx) => (val, idx))
                .OrderByDescending(val => val.val)
                .Take(runInputs.MultiObjectiveInputs.FrontMinSize)
                .Select(val => points[val.idx])
                .ToList());
        }

        return runOutputs with 
        {
            ParetoFrontOutput = new ParetoFrontOutput
            {
                Front = _front.GetPoints().ToImmutableList()
            }
        };  
    }
    
}