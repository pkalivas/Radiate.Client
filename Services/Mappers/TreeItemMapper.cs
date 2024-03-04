using Radiate.Client.Domain.Store.Models;

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
}