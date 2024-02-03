namespace Radiate.Client.Services.Store.Actions;

public record LayoutChangedAction 
{
    public bool IsSidebarOpen { get; init; }
}
