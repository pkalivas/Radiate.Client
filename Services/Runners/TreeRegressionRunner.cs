using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Models.States;
using Radiate.Data;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Extensions.Engines;
using Radiate.Extensions.Evolution.Architects;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Optimizers.Evolution.Interceptors;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public class TreeRegressionRunner : EngineRunner<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>
{
    public TreeRegressionRunner(IStore<RootState> store) : base(store) { }

    protected override async Task<EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>> Fit(RunInputsState inputs, CancellationTokenSource cts, Action<EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>> onEngineComplete)
    {
        var dataSet = await new RegressionFunction().LoadDataSet();
        var (features, targets) = dataSet;

        var data = new TensorFrame(features, targets);

        var problem = Architect.Tree<float>()
            .ToCodex()
            .ToRegressionProblem(data);
        
        var steepener = Engine.Genetic(problem).Async()
            .Setup(TreeSetup.Expression<float>())
            .Build();
        
        var windener = Engine.Genetic(problem).Async()
            .Setup(TreeSetup.Expression<float>(0.01f, 0.1f, 5))
            .Interceptors(new UniqueInterceptor<TreeGene<float>>())
            .Build();
        
        var engine = Engine.Cyclic(
                steepener.Limit(Limits.SteadyAccuracy(20)), 
                windener.Limit(Limits.Iteration(2)))
            .Limit(Limits.Seconds(5), Limits.Accuracy(0.0001f));
        
        return engine.Fit()
            .Limit(Limits.Iteration(inputs.LimitInputs.IterationLimit))
            .Peek(onEngineComplete)
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
    }

    protected override RunOutputsState MapToOutput(EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> output) => new()
    {
        EngineState = output.GetState(output.EngineId),
        EngineId = output.EngineId,
        EngineStates = output.EngineStates,
        Metrics = output.Metrics,
        TreeOutput = new TreeOutput
        {
            Type = typeof(Tree<float>).FullName,
            Trees = output.Model.Trees.Select(tree => (object)tree).ToList()
        }
    };   
}