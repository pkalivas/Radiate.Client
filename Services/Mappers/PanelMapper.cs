using Radiate.Client.Domain.Templates.Panels;

namespace Radiate.Client.Services.Mappers;

public static class PanelMapper
{
    public static List<IPanel> Flatten(List<IPanel> panels)
    {
        var result = new List<IPanel>();
        
        foreach (var panel in panels)
        {
            result.AddRange(FlattenPanel(panel));
        }
        
        return result;
    }
    
    private static IEnumerable<IPanel> FlattenPanel(IPanel panel)
    {
        var result = new List<IPanel>();
        
        if (panel is GridPanel gridPanel)
        {
            result.Add(gridPanel);
            
            foreach (var child in gridPanel.Items.Select(i => i.Panel))
            {
                result.AddRange(FlattenPanel(child));
            }
        }
        else if (panel is TabPanel tabPanel)
        {
            result.Add(tabPanel);
            
            foreach (var child in tabPanel.ChildPanels)
            {
                result.AddRange(FlattenPanel(child));
            }
        }
        else if (panel is AccordionPanel accordionPanel)
        {
            result.Add(accordionPanel);
            
            foreach (var child in accordionPanel.ChildPanels)
            {
                result.AddRange(FlattenPanel(child));
            }
        }
        else if (panel is AccordionPanelItem accPanelItem)
        {
            result.Add(accPanelItem);
        }
        else
        {
            result.Add(panel);
        }
        
        return result;
    }
}