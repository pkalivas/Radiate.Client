using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Runners.OutputTransforms;
using Radiate.Data;
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

namespace Radiate.Client.Services.Runners;

public class SinWaveGraphRunner : DataSetRunner<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public SinWaveGraphRunner(ITensorFrameService tensorFrameService, IStore<RootState> store) 
        : base(tensorFrameService, store) { }

    protected override async Task<IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> BuildEngine(RunInputsState inputs, TensorFrame frame)
    {
        var graph = Architect.Graph<float>()
            .SetOutputs(Ops.Linear<float>())
            .Build(builder => builder.Lstm(1));
        
        var problem = new GraphCodex<float>(graph).ToRegression(frame).Complexity(50);
        
        return Engine.Genetic(problem).Async()
            .Setup(EngineSetup.RecurrentNeat<float>())
            .Build()
            .Limit(Limits.Iteration(inputs.LimitInputs.IterationLimit));
    }

    protected override async Task<TensorFrame> BuildFrame(RunInputsState inputs)
    {
        var (features, targets) = await new SinWave().LoadDataSet();
        
        return new TensorFrame(features, targets)
            .Shift(5)
            .Split();    
    }
    
    protected override List<IRunOutputTransform<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> GetOutputTransforms() => 
        new()
        {
            new GraphOutputTransform(),
        };
}