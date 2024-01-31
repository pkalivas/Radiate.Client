using MudBlazor;

namespace Radiate.Client.Components.Store.Models;

public class TreeItemData<T>
{
    public string Icon { get; }
    public Color Color { get; }
    public T Data { get; }

    public bool IsExpanded { get; set; } = true;
    
    public HashSet<TreeItemData<T>> TreeItems { get; set; } = new();

    public TreeItemData(string icon, Color color, T data)
    {
        Icon = icon;
        Color = color;
        Data = data;
    }
    
    public bool HasChild => TreeItems.Count > 0;
}