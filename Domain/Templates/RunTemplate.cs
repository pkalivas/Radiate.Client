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
    public List<IPanelTemplate> Components { get; }
}

public interface IPanelTemplate
{
    public Guid Id { get; }
    public string Name { get; }
    public List<string> Actions { get; }
    public List<IPanelTemplate> Components { get; }
    public List<IPanelTemplate> Panels { get; set; }
}

public class PanelTemplate : IPanelTemplate
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public List<string> Actions { get; set; } = new();
    public List<IPanelTemplate> Components { get; set; } = new();
    public List<IPanelTemplate> Panels { get; set; } = new();
}

public class GridTemplate : IPanelTemplate
{
    public Guid Id { get; init; }
    public string Name => ComponentTypes.Grid;
    public List<string> Actions { get; set; } = new();
    public List<IPanelTemplate> Components { get; set; } = new();
    public List<IPanelTemplate> Panels { get; set; } = new();
}

public class ExpansionPanelTemplate : IPanelTemplate
{
    public Guid Id { get; init; }
    public string Name => ComponentTypes.ExpansionPanel;
    public List<string> Actions { get; set; } = new();
    public List<IPanelTemplate> Components { get; set; } = new();
    public List<IPanelTemplate> Panels { get; set; } = new();
}

public class ExpansionPanelItemTemplate : IPanelTemplate
{
    public Guid Id { get; init; }
    public string Name => ComponentTypes.ExpansionPanelItem;
    public List<string> Actions { get; set; } = new();
    public List<IPanelTemplate> Components { get; set; } = new();
    public List<IPanelTemplate> Panels { get; set; } = new();
    public bool IsExpanded { get; set; }
}

public class GridItemPanelTemplate : IPanelTemplate
{
    public Guid Id { get; init; }
    public string Name => ComponentTypes.GridItem;
    public List<string> Actions { get; set; } = new();
    public List<IPanelTemplate> Components { get; set; } = new();
    public List<IPanelTemplate> Panels { get; set; } = new();
    public int Cols { get; set; }
}
