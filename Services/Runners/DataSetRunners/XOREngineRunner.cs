using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Builders;
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
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.DataSetRunners;

public abstract class DataSetRunner<TEpoch, T> : EngineRunner<TEpoch, T> where TEpoch : IEpoch
{
    protected DataSetRunner(IStore<RootState> store) : base(store) { }
    
    protected TensorFrame? Frame { get; private set; }
    
    protected abstract Task<TensorFrame> LoadFrame();
    protected abstract TensorFrame FormatFrame(TensorFrame frame);

    protected override async Task OnStartRun(RunInputsState inputs)
    {
        Frame ??= FormatFrame(await LoadFrame());
    }
}

public abstract class XOREngineRunner<TEpoch, T> : DataSetRunner<TEpoch, T> where TEpoch : IEpoch
{
    protected XOREngineRunner(IStore<RootState> store) : base(store) { }
    
    protected override async Task<TensorFrame> LoadFrame()
    {
        var dataSet = await new XOR().LoadDataSet();
        return new TensorFrame(dataSet.Inputs, dataSet.Targets);
    }
}

public class Test : XOREngineRunner<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public Test(IStore<RootState> store) : base(store)
    {
    }

    protected override async Task<EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> Fit(RunInputsState input, CancellationTokenSource cts, 
        Action<EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> onEngineComplete)
    {
        var iterationLimit = input.LimitInputs.IterationLimit;
    
        var regression = Architect.Graph<float>().ToRegression(Frame!);
        
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

        var engine = Engine.Concat(engineOne.Limit(Limits.Iteration(20)),
                Engine.Cyclic(
                    engineTwo.Limit(Limits.SteadyAccuracy(15)),
                    engineThree.Limit(Limits.Iteration(1))))
            .Limit(Limits.Seconds(10));

        return engine.Fit()
            .Limit(Limits.Accuracy(0.01f), Limits.Iteration(iterationLimit))
            .Peek(onEngineComplete)
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
    }

    protected override RunOutputsState MapToOutput(EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> handle, RunInputsState inputs, bool isLast = false)
    {
        throw new NotImplementedException();
    }

    protected override TensorFrame FormatFrame(TensorFrame frame)
    {
        throw new NotImplementedException();
    }
}