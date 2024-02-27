using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Alterers;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Optimizers.Evolution.Interceptors;
using Radiate.Optimizers.Evolution.Selectors;
using Radiate.Optimizers.Interfaces;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.Builders;

public abstract class DataSetBuilder<TEpoch, T> : EngineBuilder<TEpoch, T>
    where TEpoch : IEpoch
    where T : IPredictionModel<T>
{
    private readonly ITensorFrameService _tensorFrameService;
    
    protected DataSetBuilder(ITensorFrameService tensorFrameService, IStore<RootState> store) : base(store)
    {
        _tensorFrameService = tensorFrameService;
    }
    
    protected abstract IEngine<TEpoch, T> BuildEngine(RunInputsState inputs, TensorFrame frame);
    protected abstract Task<TensorFrame> BuildFrame(RunInputsState inputs);

    protected override async Task<IEngine<TEpoch, T>> BuildEngine(Guid runId, RunInputsState inputs)
    {
        var frame = await BuildFrame(inputs);
        _tensorFrameService.SetTensorFrame(runId, frame);
        return BuildEngine(inputs, frame);
    }
}

public class XorGraphBuilder : DataSetBuilder<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public XorGraphBuilder(ITensorFrameService tensorFrameService, IStore<RootState> store) 
        : base(tensorFrameService, store) { }

    protected override IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> BuildEngine(RunInputsState inputs, TensorFrame frame)
    {
        var iterationLimit = inputs.LimitInputs.IterationLimit;
        
        var regression = Architect.Graph<float>().ToRegression(frame);
         
        var engineOne = Engine.Genetic(regression).Async()
            .Setup(EngineSetup.Neat<float>())
            .Build();
         
        var engineTwo = Engine.Genetic(engineOne)
            .Setup(EngineSetup.Neat<float>())
            .Build();
         
        var engineThree = Engine.Genetic(engineOne)
            .SurvivorSelector(new TournamentSelector<GraphGene<float>>())
            .Alterers(new ProgramMutator<GraphGene<float>, float>(.52f, .1f))
            .Interceptors(new UniqueInterceptor<GraphGene<float>>())
            .Build();

        return Engine.Concat(engineOne.Limit(Limits.Iteration(20)),
                Engine.Cyclic(
                    engineTwo.Limit(Limits.SteadyAccuracy(15)),
                    engineThree.Limit(Limits.Iteration(1))))
            .Limit(Limits.Seconds(10), Limits.Iteration(iterationLimit), Limits.Accuracy(0.01f));
    }

    protected override async Task<TensorFrame> BuildFrame(RunInputsState inputs)
    {
        var (features, targets) = await new XOR().LoadDataSet();
        return new TensorFrame(features, targets);
    }

    protected override RunOutputsState MapOnOutput(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output) => runOutputs with
    {
        GraphOutput = new GraphOutput
        {
            Graph = output.GetModel(),
            Type = typeof(PerceptronGraph<float>).FullName
        }
    };

    protected override RunOutputsState MapOnStop(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output) =>
        MapOnOutput(runInputs, runOutputs, output);
}