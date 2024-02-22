using Microsoft.AspNetCore.Components;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Templates;

public interface IRunTemplate
{
    public Guid Id { get; }
    public string ModelType { get; }
    public IRunUITemplate UI { get; } 
}

public interface IRunUITemplate
{
    public Guid Id { get; }
    public GridListTemplate GridList { get; }
    public AccordionTemplate LeftSideAccordion { get; }
    public AccordionTemplate RightSideAccordion { get; }
}

public class PanelTemplate
{
    public string Name { get; init; }
    public string Title { get; init; } = string.Empty;
    public List<string> Actions { get; set; } = new();
}

public class GridListTemplate
{
    public int Cols { get; set; }
    public List<GridPanelTemplate> GridPanels { get; set; } = new();
}

public class AccordionTemplate
{
    public List<ExpansionPanelTemplate> ExpansionPanels { get; set; } = new();
}

public class GridPanelTemplate
{
    public Guid Id { get; set; }
    public int Cols { get; set; }
    public int MaxHeight { get; set; } = 1000;
    public PanelTemplate GridPanel { get; set; }
}

public class ExpansionPanelTemplate
{
    public Guid Id { get; set; }
    public bool IsOpen { get; set; } = false;
    public int MaxHeight { get; set; } = 1000;
    public PanelTemplate ExpansionPanel { get; set; }
}


public class ChartPanelTemplate : PanelTemplate
{
    public string ChartType { get; set; } = ChartTypes.Line;
    public int Height { get; set; } = 125;
}

public class ToolbarPanel : PanelTemplate { }