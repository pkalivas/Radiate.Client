using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Runners.DataSetRunners;
using Radiate.Client.Services.Runners.Transforms;
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

namespace Radiate.Client.Services.Runners;

public class TreeRegressionRunner : DataSetRunner<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>
{
    public TreeRegressionRunner(ITensorFrameService tensorFrameService, IStore<RootState> store) : base(tensorFrameService, store) { }

    protected override async Task<IEngine<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>> BuildEngine(RunInputsState inputs, TensorFrame frame)
    {
         var problem = Architect.Tree<float>()
             .ToCodex()
             .ToRegressionProblem(frame);
         
         var steepener = Engine.Genetic(problem).Async()
             .Setup(TreeSetup.Expression<float>())
             .Build();
         
         var windener = Engine.Genetic(problem).Async()
             .Setup(TreeSetup.Expression<float>(0.01f, 0.1f, 5))
             .Interceptors(new UniqueInterceptor<TreeGene<float>>())
             .Build();
         
         return Engine.Cyclic(
                 steepener.Limit(Limits.SteadyAccuracy(20)), 
                 windener.Limit(Limits.Iteration(2)))
             .Limit(Limits.Seconds(5), Limits.Accuracy(0.0001f), Limits.Iteration(inputs.LimitInputs.IterationLimit));
    }

    protected override async Task<TensorFrame> BuildFrame(RunInputsState inputs)
    {
         var dataSet = await new RegressionFunction().LoadDataSet();
         var (features, targets) = dataSet;

         return new TensorFrame(features, targets);
    }
    
    protected override List<IRunOutputTransform<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>> GetOutputTransforms() => new()
    {
        new TreeOutputTransform()
    };
}
