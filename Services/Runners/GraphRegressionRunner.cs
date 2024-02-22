using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Mappers;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Harness;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Extensions.Operations;
using Radiate.Factories.Losses;
using Radiate.Tensors;
using Radiate.Tensors.Enums;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public class GraphRegressionRunner : MLEngineRunner<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public GraphRegressionRunner(IStore<RootState> store) : base(store) { }
    
    protected override async Task<TensorFrame> LoadDataSet()
    {
        var dataSet = await new RegressionFunction().LoadDataSet();
        var (features, targets) = dataSet;

        return new TensorFrame(features, targets).Transform(Norm.Normalize);
    }

    protected override async Task<EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> Fit(
        RunInputsState inputs, 
        CancellationTokenSource cts,
        Action<EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> onEngineComplete)
    {
        var problem = Architect.Graph<float>()
            .SetOutputs(Ops.Linear<float>())
            .ToCodex(Frame.CodexShape)
            .ToRegression(Frame).Complexity(50);
        
        var one = Engine.Genetic(problem).Async()
            .Setup(EngineSetup.Neat<float>())
            .Build();
        
        var two = Engine.Genetic(one)
            .Setup(EngineSetup.Neat<float>(0.01f, .2f, .1f, .1f, .1f))
            .Build();
        
        var engine = Engine.Cyclic(
                one.Limit(Limits.SteadyAccuracy(15)),
                two.Limit(Limits.Iteration(3)))
            .Limit(Limits.Seconds(15), Limits.Accuracy(0.0001f), Limits.Iteration(inputs.LimitInputs.IterationLimit));
        
        return engine.Fit()
            .Peek(onEngineComplete)
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
    }

    protected override RunOutputsState MapToOutput(
        EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output,
        RunInputsState inputs,
        bool isLast = false)
    {
        var validation = new ValidationHarness<PerceptronGraph<float>>(output.GetModel(), new MeanSquaredError()).Validate(Frame);
        
        return new()
        {
            EngineState = output.GetState(output.EngineId),
            EngineId = output.EngineId,
            EngineStates = output.EngineStates,
            Metrics = MetricMappers.GetMetricValues(output.Metrics).ToDictionary(key => key.Name),
            GraphOutput = new GraphOutput
            {
                Type = typeof(Graph<float>).FullName,
                Graph = output.GetModel().Graph
            },
            ValidationOutput = new ValidationOutput
            {
                LossFunction = validation.LossFunction,
                TrainValidation = validation.TrainValidation,
                TestValidation = validation.TestValidation
            }
        };   
    }
}