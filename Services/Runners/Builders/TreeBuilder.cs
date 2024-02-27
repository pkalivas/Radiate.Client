using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Extensions;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.Builders;

public abstract class TreeBuilder : DataSetBuilder<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>
{
    protected TreeBuilder(ITensorFrameService tensorFrameService, IStore<RootState> store) 
        : base(tensorFrameService, store) { }
    
    protected override RunOutputsState MapOnOutput(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> output) => runOutputs with
    {
        TreeOutput = new TreeOutput
        {
            Tree = output.GetModel(),
            Type = typeof(ExpressionTree<float>).FullName
        }
    };

    protected override RunOutputsState MapOnStop(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> output) =>
        MapOnOutput(runInputs, runOutputs, output);
}