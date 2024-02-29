using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
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
        
        var regression = Architect.Graph<float>().ToRegression(frame).Complexity(20);
         
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