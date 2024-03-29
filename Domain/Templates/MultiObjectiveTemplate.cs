using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Headers;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Templates;


public class MultiObjectiveTemplate : IRunTemplate
{
    public Guid Id => new("250D274E-417C-4208-B6CC-8048230EB6E9");
    public string ModelType => ModelTypes.MultiObjective;
    public IRunUITemplate UI => new MultiObjectiveUiTemplate();
}

public class MultiObjectiveUiTemplate : IRunUITemplate
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
                    Id = new Guid("24E888E2-C06F-4728-80E7-648415F577EA"),
                    ColSpan = 7,
                    Panel = new AccordionPanel
                    {
                        Id = new Guid("8FCA10DB-1073-494C-BF90-57847A736449"),
                        Toolbar = typeof(ToolBar),
                        Items =
                        [
                            new AccordionPanelItem
                            {
                                Id = new Guid("BE9001BD-ADD4-4BBA-82B9-5EB46DAC84A6"),
                                Content = typeof(MultiObjectiveInputsPanel),
                                Title = "MultiObjective",
                                Expanded = true
                            },
                            new AccordionPanelItem
                            {
                                Id = new Guid("5E191570-0983-40E8-BA03-443F8D52AF64"),
                                Content = typeof(MetricsDataGridPanel),
                                Title = "Metrics",
                                Expanded = false,
                            }
                        ]
                    }
                },
                new GridPanel.GridItem
                {
                    Id = new Guid("D92AE6D6-4F87-4561-BFED-E811BC84EBFC"),
                    ColSpan = 5,
                    Panel = new GridPanel
                    {
                        Id = new Guid("A986BBF8-1A3F-4ED0-B4C1-BE90CD59DCF3"),
                        Items =
                        [
                            new GridPanel.GridItem
                            {
                                Id = new Guid("73524501-0CBC-45F5-873A-BE669A7AD818"),
                                ColSpan = 8,
                                Panel = new BoundedPaperPanel
                                {
                                    Id = new Guid("530358FA-DE48-4449-AFEC-6FD95C65A2D7"),
                                    Content = typeof(RunSimpleStatsPanel),
                                    Height = 230,
                                    Title = "Stats",
                                }
                            },
                            new GridPanel.GridItem
                            {
                                Id = new Guid("9D8730FC-A4F8-47A5-AA35-BC65A0F3F03D"),
                                ColSpan = 4,
                                Panel = new BoundedPaperPanel
                                {
                                    Id = new Guid("252E804A-4D73-4510-BC7C-06244F30F00D"),
                                    Content = typeof(RunControlPanel),
                                    Title = "Control",
                                    Height = 230
                                }
                            },
                            new GridPanel.GridItem
                            {
                                Id = new Guid("FFA8DEDF-2247-4A8B-B1CA-7DB904C496E4"),
                                ColSpan = 12,
                                IsVisible = false,
                                Panel = new BoundedPaperPanel
                                {
                                    Id = new Guid("17207E0C-9149-47AD-9DFF-306F89044460"),
                                    Content = typeof(ParetoFrontChartPanel),
                                    Title = "Pareto Front",
                                    Height = 500
                                }
                            },
                            new GridPanel.GridItem
                            {
                                Id = new Guid("690CB090-2FFA-4445-9D21-AE2F2FE24F9B"),
                                ColSpan = 12,
                                IsVisible = false,
                                Panel = new PaperPanel
                                {
                                    Id = new Guid("935C71D8-6EE7-4846-BB19-E9DBAD0356F6"),
                                    Content = typeof(EngineStateTablePanel),
                                    Title = "Engine State",
                                }
                            }
                        ]
                    }
                },
            ]
        }
    ];
}
