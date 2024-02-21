using Microsoft.AspNetCore.Components;
using Radiate.Client.Domain.Store.Schema;

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
    public string PanelType { get; set; } = ComponentTypes.None;
    public List<string> Actions { get; set; } = new();
}

public class GridListTemplate
{
    public int Cols { get; set; }
    public List<GridPanelTemplate> GridPanels { get; set; }
}

public class AccordionTemplate
{
    public PanelTemplate ToolBarPanel { get; set; }
    public List<ExpansionPanelTemplate> ExpansionPanels { get; set; } = new();
}

public class GridPanelTemplate
{
    public Guid Id { get; set; }
    public int Rows { get; set; }
    public int Cols { get; set; }
    public PanelTemplate ToolBarPanel { get; set; }
    public PanelTemplate GridPanel { get; set; }
}

public class ExpansionPanelTemplate
{
    public Guid Id { get; set; }
    public bool IsOpen { get; set; } = false;
    public string Title { get; set; } = string.Empty;
    public PanelTemplate ExpansionPanel { get; set; }
}


//
// public interface IPanelTemplate
// {
//     public Guid Id { get; }
//     public string Name { get; }
//     public List<string> Actions { get; }
//     public List<IPanelTemplate> Components { get; }
//     public List<IPanelTemplate> Panels { get; set; }
// }
//
// public class PanelTemplate : IPanelTemplate
// {
//     public Guid Id { get; init; }
//     public string Name { get; init; }
//     public List<string> Actions { get; set; } = new();
//     public List<IPanelTemplate> Components { get; set; } = new();
//     public List<IPanelTemplate> Panels { get; set; } = new();
// }
//
// public class GridTemplate : IPanelTemplate
// {
//     public Guid Id { get; init; }
//     public string Name => ComponentTypes.Grid;
//     public List<string> Actions { get; set; } = new();
//     public List<IPanelTemplate> Components { get; set; } = new();
//     public List<IPanelTemplate> Panels { get; set; } = new();
// }
//
// public class ExpansionPanelTemplate : IPanelTemplate
// {
//     public Guid Id { get; init; }
//     public string Name => ComponentTypes.ExpansionPanel;
//     public List<string> Actions { get; set; } = new();
//     public List<IPanelTemplate> Components { get; set; } = new();
//     public List<IPanelTemplate> Panels { get; set; } = new();
// }
//
// public class ExpansionPanelItemTemplate : IPanelTemplate
// {
//     public Guid Id { get; init; }
//     public string Name => ComponentTypes.ExpansionPanelItem;
//     public List<string> Actions { get; set; } = new();
//     public List<IPanelTemplate> Components { get; set; } = new();
//     public List<IPanelTemplate> Panels { get; set; } = new();
//     public bool IsExpanded { get; set; }
// }
//
// public class GridItemPanelTemplate : IPanelTemplate
// {
//     public Guid Id { get; init; }
//     public string Name => ComponentTypes.GridItem;
//     public List<string> Actions { get; set; } = new();
//     public List<IPanelTemplate> Components { get; set; } = new();
//     public List<IPanelTemplate> Panels { get; set; } = new();
//     public int Cols { get; set; }
// }
