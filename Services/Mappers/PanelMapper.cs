using Radiate.Client.Domain.Templates.Panels;

namespace Radiate.Client.Services.Mappers;

public static class PanelMapper
{
    public static List<IPanel> Flatten(IEnumerable<IPanel> panels) => panels
        .SelectMany(FlattenPanel)
        .ToList();
    
    private static IEnumerable<IPanel> FlattenPanel(IPanel panel)
    {
        var result = new List<IPanel> { panel };

        foreach (var child in panel.ChildPanels)
        {
            result.AddRange(FlattenPanel(child));
        }
        
        return result;
    }
}