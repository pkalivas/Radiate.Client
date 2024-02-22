using System.Collections.Immutable;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Extensions;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Services.Runners.OutputTransforms;

public class TreeOutputTransform : IRunOutputTransform<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>>
{
    public RunOutputsState Transform(Guid runId, EngineOutput<GeneticEpoch<TreeGene<float>>, ExpressionTree<float>> handle,
        RunOutputsState output, RunInputsState input, bool isLast) => output with
    {
        TreeOutput = new TreeOutput
        {
            Type = typeof(ExpressionTree<float>).FullName,
            Tree = handle.GetModel()
        }
    };
}
