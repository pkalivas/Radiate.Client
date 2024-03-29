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
using Radiate.Optimizers.Evolution.Interceptors;
using Radiate.Tensors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.Regression;

public class TreeRegressionRunner : TreeRunner
{
    public TreeRegressionRunner(ITensorFrameService tensorFrameService, IStore<RootState> store) 
        : base(tensorFrameService, store) { }

    protected override IEngine<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> BuildEngine(RunInputsState inputs, TensorFrame frame)
    {
        var populationInputs = inputs.PopulationInputs;
        var treeInputs = inputs.TreeInputs;
        
        var problem = Architect.Tree<float>()
            .ToCodex()
            .ToRegression(frame);
         
        var steepener = Engine.Genetic(problem).Async()
            .Setup(TreeSetup.Expression<float>(
                populationInputs.CrossoverRate,
                populationInputs.MutationRate,
                treeInputs.MaxDepth))
            .Build();
         
        var windener = Engine.Genetic(problem).Async()
            .Setup(TreeSetup.Expression<float>(0.01f, 0.1f, treeInputs.MaxDepth))
            .Interceptors(new UniqueInterceptor<TreeGene<float>>())
            .Build();
         
        return Engine.Cyclic(
                steepener.Limit(Limits.SteadyAccuracy(20)), 
                windener.Limit(Limits.Iteration(1)))
            .Limit(Limits.Seconds(5), Limits.Accuracy(0.0001f), Limits.Iteration(inputs.LimitInputs.IterationLimit));
    }

    protected override async Task<TensorFrame> BuildFrame(RunInputsState inputs)
    {
        var dataSet = await new RegressionFunction().LoadDataSet();
        var (features, targets) = dataSet;

        return new TensorFrame(features, targets);
    }
}