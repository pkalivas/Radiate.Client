namespace Radiate.Client.Domain.Templates.Panels;

public class TabPanel : Panel
{
    public List<IPanel> Tabs { get; set; } = new();
    
    // public class TabItem
    // {
        // public IPanel Panel { get; init; }
    // }
}
