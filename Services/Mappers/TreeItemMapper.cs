using Radiate.Client.Domain.Store.Models;
using Radiate.Engines.Entities;

namespace Radiate.Client.Services.Mappers;

public static class TreeItemMapper
{
    public static HashSet<TreeItemData<T>> ToTree<T>(int index, T[] items, bool expanded = false)
        where T : ITreeItem
    {
        var currentItem = items[index];
        var seen = new HashSet<int>(new[] { index });
        var root = new TreeItemData<T>(currentItem, expanded);
        
        foreach (var child in currentItem.Children)
        {
            foreach (var item in MapToTreeRecursive(index, child, items, seen, expanded))
            {
                root.TreeItems.Add(item);
            }
        }
        
        return [root];
    }
    
    private static HashSet<TreeItemData<T>> MapToTreeRecursive<T>(int baseIndex,
        int currentIndex,
        IReadOnlyList<T> items,
        ISet<int> seen,
        bool expanded) 
        where T : ITreeItem
    {
        if (baseIndex == currentIndex)
        {
            return [];
        }

        var currentNode = items[currentIndex];
        seen.Add(currentIndex);
        var currentTreeItem = new TreeItemData<T>(currentNode, expanded);

        foreach (var child in currentNode.Children)
        {
            if (items[child].IsCyclic() && seen.Contains(child))
            {
                continue;
            }
            
            foreach (var item in MapToTreeRecursive(baseIndex, child, items, seen, expanded))
            {
                currentTreeItem.TreeItems.Add(item);
            }
        }

        return [currentTreeItem];
    }
    
    public static HashSet<TreeItemData<NodeItem>> NodeTree(int index, NodeItem[] nodeGroup, bool expanded = false)
    {
        var currentNode = nodeGroup[index];
        var seen = new HashSet<int>(new[] { index });
        var root = new TreeItemData<NodeItem>(currentNode) { IsExpanded = expanded };

        foreach (var child in currentNode.Children)
        {
            foreach (var item in GetTreeItems(index, child, nodeGroup, seen, expanded))
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
    
    private static HashSet<TreeItemData<NodeItem>> GetTreeItems(int baseIndex, 
        int index,
        IReadOnlyList<NodeItem> nodeGroup,
        ISet<int> seen,
        bool expanded = false)
    {
        if (baseIndex == index)
        {
            return [];
        }

        var currentNode = nodeGroup[index]!;
        seen.Add(index);
        var currentTreeItem = new TreeItemData<NodeItem>(currentNode) { IsExpanded = expanded };

        foreach (var child in currentNode.Children)
        {
            if (nodeGroup[child].IsRecurrent && seen.Contains(child))
            {
                continue;
            }
            
            foreach (var item in GetTreeItems(baseIndex, child, nodeGroup, seen, expanded))
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