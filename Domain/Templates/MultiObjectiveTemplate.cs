using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;

namespace Radiate.Client.Domain.Templates;


public class MultiObjectiveTemplate : IRunTemplate
{
    public Guid Id => Guid.NewGuid();
    public string ModelType => ModelTypes.Image;
    public IRunUITemplate UI => new MultiObjectiveUITemplate();
}

public class MultiObjectiveUITemplate : IRunUITemplate
{
    public List<IPanel> Panels =>
    [
        new GridPanel
        {
            Id = new Guid("496D83AB-3660-45B8-B9D6-2C8A23B66B12"),
            Items =
            [
                new GridPanel.GridItem
                {
                    ColSpan = 12,
                    Panel = new ToolbarPanel
                    {
                        Id = new Guid("8FCA10DB-1073-494C-BF90-57847A736449"),
                        Content = typeof(MultiObjectiveInputsPanel),
                        Title = "MultiObjective",
                        Actions =
                        [
                            MenuActions.Copy,
                            MenuActions.EngineTree
                        ]
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 2,
                    Panel = new CardPanel
                    {
                        Id = new Guid("365C357D-3A47-418B-84A8-7CBE2DAE1B29"),
                        Content = typeof(RunControlPanel),
                        Title = "Control",
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 8,
                    Panel = new CardPanel
                    {
                        Id = new Guid("17207E0C-9149-47AD-9DFF-306F89044460"),
                        Content = typeof(MetricsDataGridPanel),
                        Title = "Metrics",
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 4,
                    Panel = new PaperPanel
                    {
                        Id = new Guid("621B1B13-671E-4EF0-86C4-D7172BFC49D5"),
                        Content = typeof(ParetoFrontChartPanel),
                        Title = "Front",
                        Height = 500
                    }
                }
            ]
        }
    ];
}

//
// public class MultiObjectiveTemplate : IRunTemplate
// {
//     public Guid Id => Guid.NewGuid();
//     public string ModelType => ModelTypes.MultiObjective;
//     public IRunUITemplate UI => new MultiObjectiveUiTemplate();
// }
//
// public class MultiObjectiveUiTemplate : IRunUITemplate
// {
//     public Guid Id => new("AFDB4F48-1A58-4D95-85DD-530E43105E20");
//
//     public GridListTemplate GridList => new()
//     {
//         Cols = 12,
//         GridPanels = new List<GridPanelTemplate>
//         {
//             new()
//             {
//                 Id = new Guid("53EA2B0F-E26C-45A3-8DCF-06C742998CF8"),
//                 Cols = 8,
//                 GridPanel = new ToolbarPanelTemplate
//                 {
//                     Name = nameof(MultiObjectiveInputsPanel),
//                     Title = "MultiObjective",
//                     Actions = new List<string>
//                     {
//                         MenuActions.Copy,
//                         MenuActions.EngineTree
//                     }
//                 }
//             },
//             new()
//             {
//                 Id = new Guid("365C357D-3A47-418B-84A8-7CBE2DAE1B29"),
//                 Cols = 4,
//                 GridPanel = new PanelTemplate
//                 {
//                     Name = nameof(RunControlPanel)
//                 }
//             }
//         }
//     };
//
//     public AccordionTemplate LeftSideAccordion => new()
//     {
//         ExpansionPanels =
//         [
//             new()
//             {
//                 Id = new Guid("C3C00DB8-DBCB-407A-85BF-432D3591CA62"),
//                 IsOpen = true,
//                 ExpansionPanel = new PanelTemplate
//                 {
//                     Name = nameof(MetricsDataGridPanel),
//                     Title = "Metrics",
//                 }
//             }
//         ]
//     };
//
//     public AccordionTemplate RightSideAccordion { get; } = new()
//     {
//         ExpansionPanels =
//         [
//             new()
//             {
//                 Id = new Guid("F4114F2B-4C84-455C-89D5-998964CDEB1B"),
//                 IsOpen = true,
//                 MaxHeight = 500,
//                 ExpansionPanel = new PanelTemplate
//                 {
//                     Name = nameof(ParetoFrontChartPanel),
//                     Title = "Front",
//                 }
//             },
//
//             new()
//             {
//                 Id = new Guid("6BA53B93-EF5A-4E2B-ABA6-87E095854D37"),
//                 MaxHeight = 150,
//                 IsOpen = true,
//                 ExpansionPanel = new ChartPanelTemplate
//                 {
//                     Name = MetricNames.AgeDistribution,
//                     Title = MetricNames.AgeDistribution,
//                     ChartType = ChartTypes.Bar,
//                     Height = 125
//                 }
//             }
//         ],
//     };
// }