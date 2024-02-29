namespace Radiate.Client.Domain.Store.Models;

public class TreeItemData<T>
{
    public T Data { get; init; }
    public HashSet<TreeItemData<T>> TreeItems { get; init; } = new();
    public bool IsExpanded { get; set; } = false;

    public TreeItemData(T data)
    {
        Data = data;
    }
    
    public bool HasChild => TreeItems.Count > 0;
}