using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Schema;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Engines.Limits;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Optimizers.Evolution.Interceptors;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.XOR;

public class GraphXorRunner : GraphRunner
{
    public GraphXorRunner(ITensorFrameService tensorFrameService, IStore<RootState> store) 
        : base(tensorFrameService, store) { }
    
    protected override async Task<TensorFrame> BuildFrame(RunInputsState inputs)
    {
        var (features, targets) = await new Data.XOR().LoadDataSet();
        return new TensorFrame(features, targets);
    }

    protected override IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> BuildEngine(RunInputsState inputs, TensorFrame frame)
    {
        var iterationInputs = inputs.LimitInputs;
        var populationInputs = inputs.PopulationInputs;
        var graphInputs = inputs.GraphInputs;
        
        var regression = Architect.Graph<float>().ToRegression(frame, graphInputs.NodeComplexity);
         
        return Engine.Genetic(regression).Async()
            .PopulationSize(populationInputs.PopulationSize)
            .Setup(EngineSetup.Neat<float>(
                populationInputs.CrossoverRate,
                populationInputs.MutationRate,
                graphInputs.AddGateRate,
                graphInputs.AddWeightRate,
                graphInputs.AddLinkRate))
            .Interceptors(new UniqueInterceptor<GraphGene<float>>())
            .Limit(Limits.Iteration(iterationInputs.IterationLimit), Limits.Accuracy(0.01f))
            .Build();
    }

    private IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> BuildEngine2(RunInputsState inputs, TensorFrame frame, HashSet<int> seen, int currentEngineIndex = 0)
    {
        var engineInputs = inputs.EngineInputs[currentEngineIndex];

        if (engineInputs.EngineType is EngineTypes.Cyclic or EngineTypes.Concat)
        {
            var subEngines = new List<IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>>();
            foreach (var subEngineIndex in engineInputs.SubEngineInputIndexes)
            {
                if (!seen.Add(subEngineIndex))
                {
                    continue;
                }

                subEngines.Add(BuildEngine2(inputs, frame, seen, subEngineIndex));
            }

            return engineInputs.EngineType is EngineTypes.Cyclic
                ? CreateCyclicEngine(subEngines)
                    .Limit(Limits.Iteration(1000))
                : CreateConcatEngine(subEngines)
                    .Limit(Limits.Iteration(1000));
        }
        
        return BuildEngine3(engineInputs, frame);

        
        
        // foreach (var engineInputId in inputs.EngineInputs.Keys.OrderBy(key => key))
        // {
        //     var engineInputs = inputs.EngineInputs[engineInputId];
        //     
        //     seen.Add(engineInputId);
        //
        //     if (engineInputs.EngineType is EngineTypes.Cyclic or EngineTypes.Concat)
        //     {
        //         var subEngines = new List<IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>>();
        //         foreach (var subEngineId in engineInputs.SubEngineInputIndexes)
        //         {
        //             if (seen.Contains(subEngineId))
        //             {
        //                 continue;
        //             }
        //             
        //             var subEngineInputs = inputs.EngineInputs[subEngineId];
        //         }
        //     }
        //     
        //     var engine = BuildEngine(inputs, frame);
        //     
        // }
    }
    
    private IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> BuildEngine3(EngineInputs inputs, TensorFrame frame)
    {
        var iterationInputs = inputs.LimitInputs;
        var populationInputs = inputs.PopulationInputs;
        var graphInputs = inputs.GraphInputs;
        
        var regression = Architect.Graph<float>().ToRegression(frame, graphInputs.NodeComplexity);
         
        return Engine.Genetic(regression).Async()
            .PopulationSize(populationInputs.PopulationSize)
            .Setup(EngineSetup.Neat<float>(
                populationInputs.CrossoverRate,
                populationInputs.MutationRate,
                graphInputs.AddGateRate,
                graphInputs.AddWeightRate,
                graphInputs.AddLinkRate))
            .Interceptors(new UniqueInterceptor<GraphGene<float>>())
            .Limit(Limits.Iteration(iterationInputs.IterationLimit), Limits.Accuracy(0.01f))
            .Build();
    }

}