using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Schema;
using Radiate.Extensions.Evolution.Architects.Factories;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Operations;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class ModelSelectors
{
    public static readonly ISelector<RootState, OpNodeTablePanelProjection> SelectOpNodeTablePanelModel = Selectors
        .Create<RootState, RunState, OpNodeTablePanelProjection>(RunSelectors.SelectRun, run => new OpNodeTablePanelProjection
        {
            RunId = run.RunId,
            NodeGroup = run.Inputs.ModelType is ModelTypes.Graph 
                ? run.Outputs.GraphOutput.GetGraph<float>()?.Graph.ToNodes() ?? new NodeGroup<IOp<float>>()
                : run.Outputs.TreeOutput.GetTrees<float>()?.Trees
                    .Select(tree => new NodeGroup<IOp<float>>(new OpNodeFactory<Node<IOp<float>>, float>(tree.GetFactory().Values), tree
                        .Select(node => new Node<IOp<float>>(node))
                        .ToArray()))
                    .FirstOrDefault(new NodeGroup<IOp<float>>()) ?? new NodeGroup<IOp<float>>(),
            IsComplete = run.IsCompleted,
        });
}