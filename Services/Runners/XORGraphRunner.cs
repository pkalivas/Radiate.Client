using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Alterers;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Optimizers.Evolution.Interceptors;
using Radiate.Optimizers.Evolution.Selectors;
using Radiate.Tensors;
using Reflow;

namespace Radiate.Client.Services.Runners;

public class XORGraphRunner : IEngineRunner
{
    private readonly Store<RootState> _dispatcher;
    
    public XORGraphRunner(Store<RootState> dispatcher) => _dispatcher = dispatcher;
    
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
            .Peek(res => _dispatcher.Dispatch(new AddEngineOutputAction(Map(res))))
            .TakeWhile(_ => !cts.IsCancellationRequested && !token.IsCancellationRequested)
            .ToResult();
        
        _dispatcher.Dispatch(new AddEngineOutputAction(Map(result)));
        _dispatcher.Dispatch(new RunCompletedAction());
    };

    public RunInput GetInputs(RunInputsFeature feature) => new()
    {
        Inputs = new List<RunInputValue>
        {
            new("IterationLimit", feature.IterationLimit.ToString(), nameof(Int32))
        }
    };
    
    private static RunOutputsFeature Map(EngineHandle output)
    {
        return new RunOutputsFeature
        {
            EngineState = output.GetState(output.EngineId),
            EngineId = output.EngineId,
            EngineStates = output.EngineStates,
            Metrics = output.Metrics,
        };
    }

}