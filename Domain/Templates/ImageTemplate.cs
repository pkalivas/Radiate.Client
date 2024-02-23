using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;

namespace Radiate.Client.Domain.Templates;


public class ImageTemplate : IRunTemplate
{
    public Guid Id => Guid.NewGuid();
    public string ModelType => ModelTypes.Image;
    public IRunUITemplate UI => new ImageUITemplate();
}

public class ImageUITemplate : IRunUITemplate
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
                        Content = typeof(ImageInputsPanel),
                        Title = "Image",
                        Actions =
                        [
                            MenuActions.Copy,
                            MenuActions.EngineTree
                        ]
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 7,
                    Panel = new PaperPanel
                    {
                        Id = new Guid("621B1B13-671E-4EF0-86C4-D7172BFC49D5"),
                        Content = typeof(ImageTargetCurrentDisplayPanel),
                        Title = "Images",
                        Height = 300
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 3,
                    Panel = new CardPanel
                    {
                        Id = new Guid("B3234C56-9806-4E4E-ABF2-2471D90B5D91"),
                        Content = typeof(RunSimpleStatsPanel),
                        Title = "Stats",
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
                    Panel = new GridPanel
                    {
                        Id = new Guid("6A3DD486-DC80-4728-A074-A1D2D79018D9"),
                        Items =
                        [
                            new GridPanel.GridItem
                            {
                                ColSpan = 12,
                                Panel = new TabPanel
                                {
                                    Id = new Guid("9867301E-CD35-40B4-A411-4E9589CDB7DD"),
                                    Tabs = 
                                    [
                                        new TabPanel.TabItem
                                        {
                                            Panel = new PaperPanel
                                            {
                                                Id = new Guid("19687ECB-52DD-4D4A-B260-3EB55FB39B93"),
                                                Title = "Fitness Dist.",
                                                Content = typeof(MetricSummaryChartPanel),
                                                Props = new Dictionary<string, object>
                                                {
                                                    ["MetricName"] = MetricNames.FitnessDistribution,
                                                    ["ChartHeight"] = 200
                                                }
                                            }
                                        },
                                        new TabPanel.TabItem
                                        {
                                            Panel = new PaperPanel
                                            {
                                                Id = new Guid("89DBD37A-4D39-4CD0-987E-0768C2C916C8"),
                                                Title = "Age Dist.",
                                                Content = typeof(MetricSummaryChartPanel),
                                                Props = new Dictionary<string, object>
                                                {
                                                    ["MetricName"] = MetricNames.AgeDistribution,
                                                    ["ChartHeight"] = 200
                                                }
                                            }
                                        }
                                    ]
                                }
                            }
                        ]
                    }
                }
            ]
        }
    ];
}
