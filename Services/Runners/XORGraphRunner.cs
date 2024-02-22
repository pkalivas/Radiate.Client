using System.Collections.Immutable;
using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Mappers;
using Radiate.Client.Services.Runners.Transforms;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Harness;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Alterers;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Factories.Losses;
using Radiate.Optimizers.Evolution.Interceptors;
using Radiate.Optimizers.Evolution.Selectors;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public class XORGraphRunner : MLEngineRunner<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public XORGraphRunner(IStore<RootState> store) : base(store) { }
    
    protected override async Task<TensorFrame> LoadDataSet()
    {
        var (inputs, answers) = await new XOR().LoadDataSet();
        return new TensorFrame(inputs, answers);
    }

    protected override List<IRunOutputTransform<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> OutputTransforms
    {
        get;
    }

    protected override EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> Fit(RunInputsState input,
        CancellationTokenSource cts,
        Action<EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> onEngineComplete)
    {
        var iterationLimit = input.LimitInputs.IterationLimit;
    
        var regression = Architect.Graph<float>().ToRegression(Frame);
        
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

    // protected override RunOutputsState MapToOutput(
    //     EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output,
    //     RunInputsState inputs,
    //     bool isLast = false)
    // {
    //     var validation = new ValidationHarness<PerceptronGraph<float>>(output.GetModel(), new MeanSquaredError())
    //         .Validate(Frame);
    //
    //     return new()
    //     {
    //         EngineState = output.GetState(output.EngineId),
    //         EngineId = output.EngineId,
    //         EngineStates = output.EngineStates.ToImmutableDictionary(),
    //         Metrics = MetricMappers.GetMetricValues(output.Metrics).ToImmutableDictionary(key => key.Name),
    //         GraphOutput = new GraphOutput
    //         {
    //             Type = typeof(Graph<float>).FullName,
    //             Graph = output.GetModel().Graph
    //         },
    //         ValidationOutput = new ValidationOutput
    //         {
    //             LossFunction = validation.LossFunction,
    //             TrainValidation = validation.TrainValidation,
    //             TestValidation = validation.TestValidation
    //         }
    //     };
    // }
}