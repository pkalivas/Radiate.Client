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
    public TreeRegressionRunner(IDataSetService dataSetService, IStore<RootState> store) : base(dataSetService, store) { }

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

// public class TreeRegressionRunner : MLEngineRunner<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>
// {
//     public TreeRegressionRunner(IStore<RootState> store) : base(store) { }
//     
//     protected override async Task<TensorFrame> LoadDataSet()
//     {
//         var dataSet = await new RegressionFunction().LoadDataSet();
//         var (features, targets) = dataSet;
//
//         return new TensorFrame(features, targets);
//     }
//
//     protected override List<IRunOutputTransform<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>> OutputTransforms
//     {
//         get;
//     }
//
//     protected override EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> Fit(RunInputsState inputs,
//         CancellationTokenSource cts,
//         Action<EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>> onEngineComplete)
//     {
//         var problem = Architect.Tree<float>()
//             .ToCodex()
//             .ToRegressionProblem(Frame);
//         
//         var steepener = Engine.Genetic(problem).Async()
//             .Setup(TreeSetup.Expression<float>())
//             .Build();
//         
//         var windener = Engine.Genetic(problem).Async()
//             .Setup(TreeSetup.Expression<float>(0.01f, 0.1f, 5))
//             .Interceptors(new UniqueInterceptor<TreeGene<float>>())
//             .Build();
//         
//         var engine = Engine.Cyclic(
//                 steepener.Limit(Limits.SteadyAccuracy(20)), 
//                 windener.Limit(Limits.Iteration(2)))
//             .Limit(Limits.Seconds(5), Limits.Accuracy(0.0001f));
//         
//         return engine.Fit()
//             .Limit(Limits.Iteration(inputs.LimitInputs.IterationLimit))
//             .Peek(onEngineComplete)
//             .TakeWhile(_ => !cts.IsCancellationRequested)
//             .ToResult();
//     }
//
//     // protected override RunOutputsState MapToOutput(EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> output,
//     //     RunInputsState inputs,
//     //     bool isLast = false) => new()
//     // {
//     //     EngineState = output.GetState(output.EngineId),
//     //     EngineId = output.EngineId,
//     //     EngineStates = output.EngineStates.ToImmutableDictionary(),
//     //     Metrics = MetricMappers.GetMetricValues(output.Metrics).ToImmutableDictionary(key => key.Name),
//     //     TreeOutput = new TreeOutput
//     //     {
//     //         Type = typeof(Tree<float>).FullName,
//     //         Trees = output.Model.Trees.Select(tree => (object)tree).ToImmutableList()
//     //     }
//     // };
// }