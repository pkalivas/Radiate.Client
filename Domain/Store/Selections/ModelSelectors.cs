using System.Text.Json;
using Radiate.Client.Domain.Store.Models;
using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Schema;
using Radiate.Extensions.Evolution.Architects.Interfaces;
using Radiate.Extensions.Operations;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class ModelSelectors
{
    public static readonly ISelector<RootState, OpNodeTablePanelProjection> SelectOpNodeTablePanelModel = Selectors
        .Create<RootState, RunState, OpNodeTablePanelProjection>(RunSelectors.SelectRun, run =>
            new OpNodeTablePanelProjection
            {
                RunId = run.RunId,
                NodeItems = run.Inputs.ModelType switch
                {
                    ModelTypes.Graph => MapToNodeItems(run.Outputs.GraphOutput.GetGraph<float>()?.Graph),
                    ModelTypes.Tree => MapToNodeItems(run.Outputs.TreeOutput.GetTrees<float>()?.Trees.FirstOrDefault()),
                    _ => Array.Empty<NodeItem>()
                },
                IsComplete = run.IsCompleted,
            });

    private static NodeItem[] MapToNodeItems<TNode>(INodeGroup<TNode, IOp<float>>? nodeGroup)
        where TNode : INode<TNode, IOp<float>> => nodeGroup is null 
            ? Array.Empty<NodeItem>() 
            : nodeGroup
                .Select(node => new NodeItem
                {
                    NodeId = node.NodeId,
                    Index = node.Index,
                    Op = node.Value,
                    NodeType = node.NodeType,
                    Direction = node.Direction,
                    IsValid = node.IsValid,
                    IsEnabled = node.IsEnabled,
                    IsRecurrent = node.IsRecurrent,
                    Incoming = node.Incoming,
                    Outgoing = node.Outgoing,
                    Children = node.GetChildren()
                })
                .ToArray();
    
    public static readonly ISelector<RootState, ModelDownloadProjection> SelectModelDownload = Selectors
        .Create<RootState, RunState, ModelDownloadProjection>(RunSelectors.SelectRun, run =>
            new ModelDownloadProjection
            {
                RunId = run.RunId,
                IsRunning = run.IsRunning,
                JsonData = run.Inputs.ModelType switch
                {
                    ModelTypes.Graph => JsonSerializer.Serialize(run.Outputs.GraphOutput.GetGraph<float>(), new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }),
                    ModelTypes.Tree => JsonSerializer.Serialize(run.Outputs.TreeOutput.GetTrees<float>(), new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }),
                    _ => ""
                }
            });

}