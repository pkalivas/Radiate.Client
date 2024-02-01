using Radiate.Client.Components.Store;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
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

namespace Radiate.Client.Services.Runners;

public class XORGraphRunner : IEngineRunner
{
    private readonly IDispatcher _dispatcher;
    
    public XORGraphRunner(IDispatcher dispatcher) => _dispatcher = dispatcher;
    
    public Func<CancellationToken, Task> Run(RunInput command, CancellationTokenSource cts) => async token =>
    {
        var iterationLimit = command.GetInputValue<int>("IterationLimit");
        
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
            .Limit(Limits.Seconds(10));

    
        var result = engine.Fit()
            .Limit(Limits.Accuracy(0.01f), Limits.Iteration(iterationLimit))
            .Peek(res => _dispatcher.Dispatch<AddEngineOutputAction, AppFeature>(new AddEngineOutputAction(Map(res))))
            // .Peek(res => resultCallback(Map(res)))
            .TakeWhile(_ => !cts.IsCancellationRequested && !token.IsCancellationRequested)
            .ToResult();
        
        // resultCallback(Map(result));
    };

    public RunInput GetInputs(AppFeature feature) => new()
    {
        Inputs = new List<RunInputValue>
        {
            new("IterationLimit", feature.EngineInputs.IterationLimit.ToString(), nameof(Int32))
        }
    };
    
    private static EngineOutputState Map(EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output)
    {
        return new EngineOutputState
        {
            EngineState = output.GetState(output.EngineId),
            EngineId = output.EngineId,
            EngineStates = output.EngineStates,
            Metrics = output.Metrics,
            Outputs = new List<RunOutputValue>
            {
                new("Display", output.ToString(), nameof(String))
            }
        };
    }

}