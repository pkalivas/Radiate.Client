using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Extensions;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Services.Runners.Transforms;

public class TreeOutputTransform : IRunOutputTransform<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>
{
    public RunOutputsState Transform(EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> handle,
        RunOutputsState output, RunInputsState input, bool isLast) => output with
    {
        TreeOutput = new TreeOutput
        {
            Type = typeof(Tree<float>).FullName,
            Trees = handle.GetModel().Trees.Select(tree => (object)tree).ToImmutableList()
        }
    };
}
