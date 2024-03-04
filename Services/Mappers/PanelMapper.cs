using Radiate.Client.Domain.Templates.Panels;

namespace Radiate.Client.Services.Mappers;

public static class PanelMapper
{
    public static List<T> Flatten<T>(List<IPanel> panels, Func<IPanel, T> mapper) => panels
        .SelectMany(p => FlattenPanel(p, mapper))
        .ToList();
    
    private static IEnumerable<T> FlattenPanel<T>(IPanel panel, Func<IPanel, T> mapper)
    {
        var result = new List<T>();
        
        if (panel is GridPanel gridPanel)
        {
            result.Add(mapper(gridPanel));
            
            foreach (var child in gridPanel.Items.Select(i => i.Panel))
            {
                result.AddRange(FlattenPanel(child, mapper));
            }
        }
        else if (panel is TabPanel tabPanel)
        {
            result.Add(mapper(tabPanel));
            
            foreach (var child in tabPanel.ChildPanels)
            {
                result.AddRange(FlattenPanel(child, mapper));
            }
        }
        else if (panel is AccordionPanel accordionPanel)
        {
            result.Add(mapper(accordionPanel));
            
            foreach (var child in accordionPanel.ChildPanels)
            {
                result.AddRange(FlattenPanel(child, mapper));
            }
        }
        else if (panel is AccordionPanelItem accPanelItem)
        {
            result.Add(mapper(accPanelItem));
        }
        else
        {
            result.Add(mapper(panel));
        }
        
        return result;
    }
}