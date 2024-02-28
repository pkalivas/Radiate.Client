using MudBlazor;
using Radiate.Client.Domain.Store.Models;
using Radiate.Engines.Entities;
using Radiate.Engines.Schema;
using Radiate.Extensions.Evolution.Architects.Interfaces;
using Radiate.Extensions.Schema;

namespace Radiate.Client.Services.Mappers;

public static class TreeItemMapper
{
    public static HashSet<TreeItemData<TNode>> NodeTree<TGroup, TNode, T>(int index, INodeGroup<TGroup, TNode, T> nodeGroup)
        where TGroup : INodeGroup<TGroup, TNode, T>
        where TNode : INode<TNode, T>
    {
        var currentNode = nodeGroup[index];
        var seen = new HashSet<int>(new[] { index });
        var root = new TreeItemData<TNode>(GetIcon<TNode, T>(currentNode), GetColor<TNode, T>(currentNode), currentNode);

        foreach (var child in currentNode.GetChildren())
        {
            foreach (var item in GetTreeItems(index, child, nodeGroup, seen))
            {
                root.TreeItems.Add(item);
            }
        }

        return [root];
    }
    
    private static HashSet<TreeItemData<TNode>> GetTreeItems<TGroup, TNode, T>(int baseIndex,
        int index,
        INodeGroup<TGroup, TNode, T> nodeGroup,
        HashSet<int> seen)
        where TGroup : INodeGroup<TGroup, TNode, T>
        where TNode : INode<TNode, T>
    {
        if (baseIndex == index)
        {
            return [];
        }

        var currentNode = nodeGroup[index]!;
        seen.Add(index);
        var currentTreeItem = new TreeItemData<TNode>(GetIcon<TNode, T>(currentNode),
            GetColor<TNode, T>(currentNode),
            currentNode);

        foreach (var child in currentNode.GetChildren())
        {
            if (nodeGroup[child].IsRecurrent && seen.Contains(child))
            {
                continue;
            }
            
            foreach (var item in GetTreeItems(baseIndex, child, nodeGroup, seen))
            {
                currentTreeItem.TreeItems.Add(item);
            }
        }

        return [currentTreeItem];
    }

    public static HashSet<TreeItemData<EngineState>> GetItems(IReadOnlyDictionary<string, EngineState> states, 
        IReadOnlyDictionary<string, bool> expanded)
    {
        var seen = new HashSet<string>();
        var result = new HashSet<TreeItemData<EngineState>>();

        foreach (var state in states.Values)
        {
            if (seen.Contains(state.EngineId))
            {
                continue;
            }
            
            foreach (var item in GetTreeItems(state.EngineId, seen, states, expanded))
            {
                result.Add(item);
            }
        }
        
        return result;
    }
    
    private static HashSet<TreeItemData<EngineState>> GetTreeItems(string current, 
        ISet<string> seen, 
        IReadOnlyDictionary<string, EngineState> states,
        IReadOnlyDictionary<string, bool> expanded)
    {
        seen.Add(current);
        
        var currentEngineState = states[current];
        var currentTreeItem = new TreeItemData<EngineState>(GetIcon(currentEngineState), GetColor(currentEngineState), currentEngineState);
        
        currentTreeItem.IsExpanded = expanded.GetValueOrDefault(current, false);
        
        foreach (var sub in states[current].SubEngines)
        {
            foreach (var item in GetTreeItems(sub, seen, states, expanded))
            {
                currentTreeItem.TreeItems.Add(item);
            }
        }
        
        return new List<TreeItemData<EngineState>> { currentTreeItem }.ToHashSet();
    }
    
    private static string GetIcon(EngineState state) => state.State switch
    {
        EngineStateTypes.Pending => Icons.Material.Filled.Pending,
        EngineStateTypes.Started => Icons.Material.Filled.Start,
        EngineStateTypes.Running => Icons.Material.Filled.RunCircle,
        EngineStateTypes.Stopped => Icons.Material.Filled.Stop,
        _ => Icons.Custom.FileFormats.FileCode
    };
    
    private static Color GetColor(EngineState state) => state.State switch
    {
        EngineStateTypes.Pending => Color.Default,
        EngineStateTypes.Started => Color.Primary,
        EngineStateTypes.Running => Color.Success,
        EngineStateTypes.Stopped => Color.Secondary,
        _ => Color.Default
    };

    private static string GetIcon<TNode, T>(TNode node)
        where TNode : INode<TNode, T> => node.NodeType switch
    {
        NodeTypes.Input => Icons.Material.Outlined.SettingsInputComponent,
        NodeTypes.Gate => Icons.Material.Outlined.Functions,
        NodeTypes.Weight => Icons.Material.Outlined.Numbers,
        NodeTypes.Link => Icons.Material.Outlined.Link,
        NodeTypes.Memory => Icons.Material.Outlined.Memory,
        NodeTypes.Output => Icons.Material.Outlined.Output,
        _ => Icons.Material.Outlined.Label
    };

    private static Color GetColor<TNode, T>(TNode node)
        where TNode : INode<TNode, T> => node.NodeType switch
    {
        NodeTypes.Input => Color.Success,
        NodeTypes.Gate => Color.Primary,
        NodeTypes.Weight => Color.Dark,
        NodeTypes.Link => Color.Info,
        NodeTypes.Memory => Color.Warning,
        NodeTypes.Output => Color.Secondary,
        _ => Color.Default
    };
}