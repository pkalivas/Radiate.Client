using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Models;
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

public class XORGraphRunner : EngineRunner<RunInputsModel>
{
    public XORGraphRunner(Store<RootState> store) : base(store) { }

     public override RunInput GetInputs(RunInputsModel model) => new()
    {
        Inputs = new List<RunInputValue>
        {
            new("IterationLimit", model.IterationLimit.ToString(), nameof(Int32))
        }
    };

    protected override RunOutputsModel MapToOutput(EngineHandle output)
    {
        return new RunOutputsModel
        {
            EngineState = output.GetState(output.EngineId),
            EngineId = output.EngineId,
            EngineStates = output.EngineStates,
            Metrics = output.Metrics,
        };    
    }

    protected override async Task<EngineHandle> Fit(RunInput input, CancellationTokenSource cts, Action<EngineHandle> onEngineComplete)
    {
        var iterationLimit = input.GetInputValue<int>("IterationLimit");
        
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

        var result = engine.Fit()
            .Limit(Limits.Accuracy(0.01f), Limits.Iteration(iterationLimit))
            .Peek(res => onEngineComplete(res))
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
        
        return result;
    }
}