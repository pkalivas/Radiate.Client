using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;

namespace Radiate.Client.Domain.Templates;


public class TreeTemplate : IRunTemplate
{
    public Guid Id => Guid.NewGuid();
    public string ModelType => ModelTypes.Tree;
    public IRunUITemplate UI => new TreeUiTemplate();
}

public class TreeUiTemplate : IRunUITemplate
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
                    ColSpan = 7,
                    Panel = new ToolbarPanel
                    {
                        Id = new Guid("8FCA10DB-1073-494C-BF90-57847A736449"),
                        Content = typeof(TreeInputsPanel),
                        Title = "Tree",
                        Actions =
                        [
                            MenuActions.Copy,
                            MenuActions.EngineTree
                        ]
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 3,
                    Panel = new PaperPanel
                    {
                        Id = new Guid("B3234C56-9806-4E4E-ABF2-2471D90B5D91"),
                        Content = typeof(RunSimpleStatsPanel),
                        Title = "Stats",
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 2,
                    Panel = new PaperPanel
                    {
                        Id = new Guid("365C357D-3A47-418B-84A8-7CBE2DAE1B29"),
                        Content = typeof(RunControlPanel),
                        Title = "Control",
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 4,
                    Panel = new PaperPanel
                    {
                        Id = new Guid("A5F2CFCD-9BB6-4511-84C3-9AB72CA8EF75"),
                        Content = typeof(AccuracyChartPanel),
                        Title = "Accuracy",
                        Height = 250,
                        Props = new Dictionary<string, object>
                        {
                            ["Height"] = 200
                        }
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 4,
                    Panel = new PaperPanel
                    {
                        Id = new Guid("1AB95A03-9B5D-4D7C-B0EB-06797C2367F3"),
                        Content = typeof(ScorePanel),
                        Title = "Score",
                        Height = 250,
                        Props = new Dictionary<string, object>
                        {
                            ["Height"] = 200
                        }
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 4,
                    Panel = new PaperPanel
                    {
                        Id = new Guid("E3F6D540-AA06-449E-B503-456930DCB8EA"),
                        Content = typeof(ValidationPanel),
                        Title = "Validation",
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 8,
                    Panel = new TabPanel
                    {
                        Id = new Guid("57B2E6CA-DF7D-4E87-9D67-ACC68327F694"),
                        Tabs =
                        [
                            new PaperPanel
                            {
                                DisplayHeader = false,
                                Height = 500,
                                Id = new Guid("17207E0C-9149-47AD-9DFF-306F89044460"),
                                Title = "Metrics",
                                Content = typeof(MetricsDataGridPanel),
                            },
                            new PaperPanel
                            {
                                DisplayHeader = false,
                                Height = 500,
                                Id = new Guid("610F9B58-6680-4C79-99DB-14AFAC93BF9F"),
                                Title = "Tree Nodes",
                                Content = typeof(OpNodeTablePanel),
                            }
                        ]
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
                                         new PaperPanel
                                        {
                                            Id = new Guid("19687ECB-52DD-4D4A-B260-3EB55FB39B93"),
                                            Title = "Fitness Dist.",
                                            Content = typeof(MetricSummaryChartPanel),
                                            Props = new Dictionary<string, object>
                                            {
                                                ["MetricName"] = MetricNames.FitnessDistribution,
                                                ["ChartHeight"] = 200
                                            }
                                        },
                                        new PaperPanel
                                        {
                                            Id = new Guid("89DBD37A-4D39-4CD0-987E-0768C2C916C8"),
                                            Title = "Age Dist.",
                                            Content = typeof(MetricSummaryChartPanel),
                                            Props = new Dictionary<string, object>
                                            {
                                                ["MetricName"] = MetricNames.AgeDistribution,
                                                ["ChartHeight"] = 200
                                            }
                                        },
                                        new PaperPanel
                                        {
                                            Id = new Guid("FA2CB9C6-54FE-40AF-AA4D-2CD538B82B36"),
                                            Title = "Genome Size",
                                            Content = typeof(MetricSummaryChartPanel),
                                            Props = new Dictionary<string, object>
                                            {
                                                ["MetricName"] = MetricNames.GenomeSizeDistribution,
                                                ["ChartHeight"] = 200
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