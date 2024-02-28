using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Engines.Limits;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Codex;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Extensions.Operations;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.SinWave;

public class GraphSinWaveRunner : GraphRunner
{
    public GraphSinWaveRunner(ITensorFrameService tensorFrameService, IStore<RootState> store) 
        : base(tensorFrameService, store) { }

    protected override IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> BuildEngine(RunInputsState inputs, TensorFrame frame)
    {
        var populationInputs = inputs.PopulationInputs;
        var graphInputs = inputs.GraphInputs;
        var iterationInputs = inputs.LimitInputs;
        
        var graph = Architect.Graph<float>()
            .SetOutputs(Ops.Linear<float>())
            .Build(builder => builder.Lstm(1));
        
        var problem = new GraphCodex<float>(graph).ToRegression(frame).Complexity(50);

        return Engine.Genetic(problem).Async()
            .PopulationSize(populationInputs.PopulationSize)
            .Setup(EngineSetup.RecurrentNeat<float>(
                graphInputs.AddGateRate,
                graphInputs.AddWeightRate,
                graphInputs.AddMemoryRate,
                graphInputs.AddLinkRate))
            .Limit(Limits.Iteration(iterationInputs.IterationLimit))
            .Build();
    }

    protected override async Task<TensorFrame> BuildFrame(RunInputsState inputs)
    {
        var (features, targets) = await new Data.SinWave().LoadDataSet();
        
        return new TensorFrame(features, targets)
            .Shift(5)
            .Split();      
    }
}