using Radiate.Client.Domain.Store.Models;

namespace Radiate.Client.Services.Mappers;

public static class TreeItemMapper
{
    public static HashSet<TreeItem<T, TKey>> ToTree<T, TKey>(IReadOnlyDictionary<TKey, T> items, TKey index = default!, bool expanded = false)
        where T : ITreeItem<TKey>
    {
        if (items.Count == 0)
        {
            return new();
        }
        
        var currentItem = items[index];
        var seen = new HashSet<TKey>(new[] { index });
        var root = new TreeItem<T, TKey>(currentItem, expanded);
        
        foreach (var child in currentItem.Children)
        {
            foreach (var item in MapToTreeRecursive(index, child, items, seen, expanded))
            {
                root.ChildItems.Add(item);
            }
        }
        
        return [root];
    }
    
    private static HashSet<TreeItem<T, TKey>> MapToTreeRecursive<T, TKey>(TKey baseIndex,
        TKey currentIndex,
        IReadOnlyDictionary<TKey, T> items,
        ISet<TKey> seen,
        bool expanded) 
        where T : ITreeItem<TKey>
    {
        if (baseIndex!.Equals(currentIndex))
        {
            return new();
        }

        var currentNode = items[currentIndex];
        seen.Add(currentIndex);
        var currentTreeItem = new TreeItem<T, TKey>(currentNode, expanded);

        foreach (var child in currentNode.Children)
        {
            if (items[child].IsCyclic() && seen.Contains(child))
            {
                continue;
            }
            
            foreach (var item in MapToTreeRecursive(baseIndex, child, items, seen, expanded))
            {
                currentTreeItem.ChildItems.Add(item);
            }
        }

        return [currentTreeItem];
    }
}