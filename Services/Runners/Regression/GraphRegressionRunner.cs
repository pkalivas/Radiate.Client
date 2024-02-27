using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Engines.Limits;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Extensions.Operations;
using Radiate.Tensors;
using Radiate.Tensors.Enums;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.Regression;

public class GraphRegressionRunner : GraphRunner
{
    public GraphRegressionRunner(ITensorFrameService tensorFrameService, IStore<RootState> store)
        : base(tensorFrameService, store) { }

    protected override IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> BuildEngine(RunInputsState inputs, TensorFrame frame)
    {
        var problem = Architect.Graph<float>()
            .SetOutputs(Ops.Linear<float>())
            .ToCodex(frame.CodexShape)
            .ToRegression(frame).Complexity(50);
         
        var one = Engine.Genetic(problem).Async()
            .Setup(EngineSetup.Neat<float>())
            .Build();
         
        var two = Engine.Genetic(one)
            .Setup(EngineSetup.Neat<float>(0.01f, .2f, .1f, .1f, .1f))
            .Build();
         
        return Engine.Cyclic(
                one.Limit(Limits.SteadyAccuracy(15)),
                two.Limit(Limits.Iteration(3)))
            .Limit(Limits.Seconds(15), Limits.Accuracy(0.0001f), Limits.Iteration(inputs.LimitInputs.IterationLimit));
    }

    protected override async Task<TensorFrame> BuildFrame(RunInputsState inputs)
    {
        var dataSet = await new RegressionFunction().LoadDataSet();
        var (features, targets) = dataSet;

        return new TensorFrame(features, targets).Transform(Norm.Normalize);
    }
}