using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.States;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Alterers;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Optimizers.Evolution.Interceptors;
using Radiate.Optimizers.Evolution.Selectors;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public class XORGraphRunner : EngineRunner<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public XORGraphRunner(IStore<RootState> store) : base(store) { }

    protected override async Task<EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> Fit(RunInputsState input,
        CancellationTokenSource cts,
        Action<EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> onEngineComplete)
    {
        var iterationLimit = input.LimitInputs.IterationLimit;
        
        var (inputs, answers) = await new XOR().LoadDataSet();
        var frame = new TensorFrame(inputs, answers);
    
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

        var engine = Engine.Concat(engineOne.Limit(Limits.Iteration(20)),
                Engine.Cyclic(
                    engineTwo.Limit(Limits.SteadyAccuracy(15)),
                    engineThree.Limit(Limits.Iteration(1))))
            .Limit(Limits.Seconds(1000));

        return engine.Fit()
            .Limit(Limits.Accuracy(0.01f), Limits.Iteration(iterationLimit))
            .Peek(onEngineComplete)
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
    }

    protected override RunOutputsState MapToOutput(EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output) => new()
    {
        EngineState = output.GetState(output.EngineId),
        EngineId = output.EngineId,
        EngineStates = output.EngineStates,
        Metrics = output.Metrics,
        GraphOutput = new GraphOutput
        {
            Type = typeof(Graph<float>).FullName,
            Graph = output.GetModel().Graph
        }
    };    
}