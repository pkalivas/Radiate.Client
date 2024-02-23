using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Templates;

// public interface IRunTemplate
// {
//     public Guid Id { get; }
//     public string ModelType { get; }
//     public IRunUITemplate UI { get; } 
// }

// public interface IRunUITemplate
// {
//     public Guid Id { get; }
//     public GridListTemplate GridList { get; }
//     public AccordionTemplate LeftSideAccordion { get; }
//     public AccordionTemplate RightSideAccordion { get; }
// }

// public class PanelTemplate
// {
//     public string Name { get; init; }
//     public string Title { get; init; } = string.Empty;
//     public List<string> Actions { get; set; } = new();
// }
//
// public class GridListTemplate
// {
//     public int Cols { get; set; }
//     public List<GridPanelTemplate> GridPanels { get; set; } = new();
// }
//
// public class AccordionTemplate
// {
//     public List<ExpansionPanelTemplate> ExpansionPanels { get; set; } = new();
// }
//
// public class GridPanelTemplate
// {
//     public Guid Id { get; set; }
//     public int Cols { get; set; }
//     public int MaxHeight { get; set; } = 1000;
//     public PanelTemplate GridPanel { get; set; }
// }
//
// public class ExpansionPanelTemplate
// {
//     public Guid Id { get; set; }
//     public bool IsOpen { get; set; } = false;
//     public int MaxHeight { get; set; } = 1000;
//     public PanelTemplate ExpansionPanel { get; set; }
// }
//
//
// public class ChartPanelTemplate : PanelTemplate
// {
//     public string ChartType { get; set; } = ChartTypes.Line;
//     public int Height { get; set; } = 125;
// }
//
// public class ToolbarPanelTemplate : PanelTemplate { }
//
// public class CardPanelTemplate : PanelTemplate { }
//
// public class TabPanelTemplate : PanelTemplate
// {
//     public List<PanelTemplate> Tabs { get; set; } = new();
// }


////

public interface IRunTemplate
{
    public Guid Id { get; }
    public string ModelType { get; }
    public IRunUITemplate UI { get; }
}

public interface IPanel
{
    Guid Id { get; }
    string Name { get; }
    string Title { get; }
    List<string> Actions { get; }
}

public interface IRunUITemplate
{
    List<IPanel> Panels { get; }
}

public abstract class Panel : IPanel
{
    public Guid Id { get; init; }
    public string Name { get; init; }
    public string Title { get; init; } = string.Empty;
    public int Cols { get; init; } = 3;
    public List<string> Actions { get; set; } = new();
}

public class PaperPanel : Panel { }

public class CardPanel : Panel { }

public class GridPanel : Panel
{
    public int Cols { get; set; }
    public List<GridItem> Panels { get; set; } = new();

    public class GridItem
    {
        public int ColSpan { get; init; }
        public IPanel Panel { get; init; }
    }
}

public class Accordion : Panel { }

public class TabPanel : Panel
{
    public List<TabItem> Tabs { get; set; } = new();
    
    public class TabItem
    {
        public IPanel Panel { get; init; }
    }
}

public class Test : IRunTemplate
{
    public Guid Id { get; }
    public string ModelType { get; }
    public IRunUITemplate UI => new TestUI();
}

public class TestUI : IRunUITemplate
{
    public List<IPanel> Panels =>
    [
        new GridPanel
        {
            Id = new Guid("496D83AB-3660-45B8-B9D6-2C8A23B66B12"),
            Name = "GridPanel",
            Cols = 12,
            Panels =
            [
                new GridPanel.GridItem
                {
                    ColSpan = 7,
                    Panel = new TabPanel
                    {
                        Id = new Guid("53EA2B0F-E26C-45A3-8DCF-06C742998CF8"),
                        Tabs =
                        [
                            new()
                            {
                                Panel = new PaperPanel
                                {
                                    Id = new Guid("4B5E7C33-5F17-4CBE-A2E0-783EE9663693"),
                                    Name = nameof(AccuracyChartPanel),
                                    Title = "Accuracy"
                                }
                            },
                            new()
                            {
                                Panel = new PaperPanel
                                {
                                    Id = new Guid("4B5E7C33-5F17-4CBE-A2E0-783EE9663693"),
                                    Name = nameof(ScorePanel),
                                    Title = "Score"
                                }
                            }
                        ]
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 3,
                    Panel = new CardPanel
                    {
                        Id = new Guid("B3234C56-9806-4E4E-ABF2-2471D90B5D91"),
                        Name = nameof(RunSimpleStatsPanel),
                        Title = "Stats"
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 2,
                    Panel = new CardPanel
                    {
                        Id = new Guid("365C357D-3A47-418B-84A8-7CBE2DAE1B29"),
                        Name = nameof(RunControlPanel),
                        Title = "Control"
                    }
                }
            ]
        }
    ];
}