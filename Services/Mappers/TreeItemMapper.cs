using MudBlazor;
using Radiate.Client.Domain.Store.Models;
using Radiate.Engines.Entities;
using Radiate.Engines.Schema;
using Radiate.Extensions.Evolution.Architects.Interfaces;

namespace Radiate.Client.Services.Mappers;

public static class TreeItemMapper
{
    public static HashSet<TreeItemData<TNode>> NodeTree<TGroup, TNode, T>(int index, INodeGroup<TGroup, TNode, T> nodeGroup)
        where TGroup : INodeGroup<TGroup, TNode, T>
        where TNode : INode<TNode, T>
    {
        var result = new HashSet<TreeItemData<TNode>>();
        
        foreach (var child in nodeGroup[index]!.GetChildren())
        {
            foreach (var item in GetTreeItems(index, child, nodeGroup))
            {
                result.Add(item);
            }
        }

        return result;
    }
    
    private static HashSet<TreeItemData<TNode>> GetTreeItems<TGroup, TNode, T>(int baseIndex, int index, INodeGroup<TGroup, TNode, T> nodeGroup)
        where TGroup : INodeGroup<TGroup, TNode, T>
        where TNode : INode<TNode, T>
    {
        if (baseIndex == index)
        {
            return [];
        }

        var currentNode = nodeGroup[index]!;
        
        var currentTreeItem = new TreeItemData<TNode>(Icons.Material.Outlined.Folder, Color.Default, currentNode)
        {
            IsExpanded = true
        };

        foreach (var child in currentNode.GetChildren())
        {
            foreach (var item in GetTreeItems(baseIndex, child, nodeGroup))
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
}