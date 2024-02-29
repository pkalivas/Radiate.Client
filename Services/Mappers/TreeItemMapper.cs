using Radiate.Client.Domain.Store.Models;
using Radiate.Engines.Entities;

namespace Radiate.Client.Services.Mappers;

public static class TreeItemMapper
{
    public static HashSet<TreeItemData<NodeItem>> NodeTree(int index, NodeItem[] nodeGroup)
    {
        var currentNode = nodeGroup[index];
        var seen = new HashSet<int>(new[] { index });
        var root = new TreeItemData<NodeItem>(currentNode);

        foreach (var child in currentNode.Children)
        {
            foreach (var item in GetTreeItems(index, child, nodeGroup, seen))
            {
                root.TreeItems.Add(item);
            }
        }

        return [root];
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
    
    private static HashSet<TreeItemData<NodeItem>> GetTreeItems(int baseIndex, int index, NodeItem[] nodeGroup, HashSet<int> seen)
    {
        if (baseIndex == index)
        {
            return [];
        }

        var currentNode = nodeGroup[index]!;
        seen.Add(index);
        var currentTreeItem = new TreeItemData<NodeItem>(currentNode);

        foreach (var child in currentNode.Children)
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
    
    private static HashSet<TreeItemData<EngineState>> GetTreeItems(string current, 
        ISet<string> seen, 
        IReadOnlyDictionary<string, EngineState> states,
        IReadOnlyDictionary<string, bool> expanded)
    {
        seen.Add(current);
        
        var currentEngineState = states[current];
        var currentTreeItem = new TreeItemData<EngineState>(currentEngineState);
        
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
}