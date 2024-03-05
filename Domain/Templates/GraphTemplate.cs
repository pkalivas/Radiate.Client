using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Headers;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;

namespace Radiate.Client.Domain.Templates;

public class GraphTemplate : IRunTemplate
{
    public Guid Id => Guid.NewGuid();
    public string ModelType => ModelTypes.Graph;
    public IRunUITemplate UI => new GraphUiTemplate();
}

public class GraphUiTemplate : IRunUITemplate
{
    public List<IPanel> Panels =>
    [
        new GridPanel
        {
            Id = new Guid("7C70DB70-34D6-4317-A3B7-E9A2111C8C9C"),
            Items = 
            [
                new GridPanel.GridItem
                {
                    Id = new Guid("D50724F1-9386-4FAB-A0D0-F19A52BFE4F8"),
                    ColSpan = 7,
                    Panel = new AccordionPanel
                    {
                        Id = new Guid("598522C1-38DA-4E20-995A-B1DB1400BFEB"),
                        Toolbar = typeof(ToolBar),
                        Items = 
                        [
                            new AccordionPanelItem
                            {
                                Id = new Guid("88D6BD18-DD38-4AD9-8064-43D0665ADD99"),
                                Expanded = true,
                                Content = typeof(GraphInputsPanel),
                                Title = "Inputs",
                            },
                            new AccordionPanelItem
                            {
                                Id = new Guid("0277DCC6-B002-409F-A62A-C9B29F564419"),
                                Expanded = false,
                                Content = typeof(TrainTestValidation),
                                Title = "Train/Test"
                            },
                            new AccordionPanelItem
                            {
                                Id = new Guid("4F263A18-446A-455D-BC37-698F67DE5846"),
                                Expanded = false,
                                Content = typeof(NodeTablePanel),
                                Title = "Nodes"
                            },
                            new AccordionPanelItem
                            {
                                Id = new Guid("B5D9CE53-97D5-41AE-8C96-25DF851C57CF"),
                                Expanded = false,
                                Content = typeof(MetricsDataGridPanel),
                                Title = "Metrics"
                            }
                        ]
                    }
                },
                new GridPanel.GridItem
                {
                    Id = new Guid("BBE31A85-C234-4AA9-A2EC-D2390F6F9E84"),
                    ColSpan = 5,
                    Panel = new GridPanel
                    {
                        Id = new Guid("11F7F65E-9BE1-4D61-AD61-62C8D93A592B"),
                        Items = 
                        [
                            new GridPanel.GridItem
                            {
                                Id = new Guid("9A7B27AA-1182-40B7-9DC9-B6BDD3E5D975"),
                                ColSpan = 12,
                                Panel = new GridPanel
                                {
                                    Id = new Guid("403E7206-2E73-4D1A-BCC9-19E1C28267F6"),
                                    Items = 
                                    [
                                        new GridPanel.GridItem
                                        {
                                            ColSpan = 7,
                                            Id = new Guid("AA67511A-EDA6-46AE-AE11-3FF289E352F7"),
                                            Panel = new BoundedPaperPanel
                                            {
                                                Id = new Guid("B3234C56-9806-4E4E-ABF2-2471D90B5D91"),
                                                Height = 230,
                                                Content = typeof(RunSimpleStatsPanel),
                                                Title = "Stats",
                                            }
                                        },
                                        new GridPanel.GridItem
                                        {
                                            Id = new Guid("D33FC873-4AF1-44EC-A45E-E0FA4AF3911E"),
                                            ColSpan = 5,
                                            Panel = new BoundedPaperPanel
                                            {
                                                Id = new Guid("365C357D-3A47-418B-84A8-7CBE2DAE1B29"),
                                                Height = 230,
                                                Content = typeof(RunControlPanel),
                                                Title = "Control",
                                            }
                                        },
                                    ]
                                }
                            },
                            new GridPanel.GridItem
                            {
                                Id = new Guid("AE1CEB03-80B1-4415-9BBD-3A9D584A5283"),
                                ColSpan = 12,
                                IsVisible = false,
                                Panel = new TabPanel
                                {
                                    Id = new Guid("8ABC3316-1156-47EA-A573-381B2EDF7AE3"),
                                    ChildPanels = 
                                    [
                                        new BoundedPaperPanel
                                        {
                                            Id = new Guid("A5F2CFCD-9BB6-4511-84C3-9AB72CA8EF75"),
                                            Content = typeof(AccuracyChartPanel),
                                            Title = "Accuracy",
                                            Height = 250,
                                            DisplayHeader = false,
                                            Props = new Dictionary<string, object>
                                            {
                                                ["Height"] = 200
                                            }
                                        },
                                        new BoundedPaperPanel
                                        {
                                            Id = new Guid("1AB95A03-9B5D-4D7C-B0EB-06797C2367F3"),
                                            Content = typeof(ScorePanel),
                                            Title = "Score",
                                            Height = 250,
                                            DisplayHeader = false,
                                            Props = new Dictionary<string, object>
                                            {
                                                ["Height"] = 200
                                            }
                                        }
                                    ]
                                }
                            },
                            new GridPanel.GridItem
                            {
                                Id = new Guid("2868E336-7E47-47A2-9DF7-CF5F6B341A01"),
                                ColSpan = 12,
                                IsVisible = false,
                                Panel = new TabPanel
                                {
                                    Id = new Guid("9867301E-CD35-40B4-A411-4E9589CDB7DD"),
                                    ChildPanels = 
                                    [
                                        new BoundedPaperPanel
                                        {
                                            Id = new Guid("19687ECB-52DD-4D4A-B260-3EB55FB39B93"),
                                            Title = "Fitness Dist.",
                                            Content = typeof(MetricSummaryChartPanel),
                                            Height = 250,
                                            Props = new Dictionary<string, object>
                                            {
                                                ["MetricName"] = MetricNames.FitnessDistribution,
                                                ["ChartHeight"] = 200
                                            }
                                        },
                                        new BoundedPaperPanel
                                        {
                                            Id = new Guid("89DBD37A-4D39-4CD0-987E-0768C2C916C8"),
                                            Title = "Age Dist.",
                                            Height = 250,
                                            Content = typeof(MetricSummaryChartPanel),
                                            Props = new Dictionary<string, object>
                                            {
                                                ["MetricName"] = MetricNames.AgeDistribution,
                                                ["ChartHeight"] = 200
                                            }
                                        },
                                        new BoundedPaperPanel
                                        {
                                            Id = new Guid("FA2CB9C6-54FE-40AF-AA4D-2CD538B82B36"),
                                            Title = "Genome Size",
                                            Height = 250,
                                            Content = typeof(MetricSummaryChartPanel),
                                            Props = new Dictionary<string, object>
                                            {
                                                ["MetricName"] = MetricNames.GenomeSizeDistribution,
                                                ["ChartHeight"] = 200
                                            }
                                        }
                                    ]
                                }
                            },
                            new GridPanel.GridItem
                            {
                                Id = new Guid("CFE22489-E00E-433F-82F1-13ED04322E83"),
                                ColSpan = 12,
                                IsVisible = false,
                                Panel = new PaperPanel
                                {
                                    Id = new Guid("48623FCA-577B-4DFE-98ED-AFE905D11CDB"),
                                    Content = typeof(EngineStateTablePanel),
                                    Title = "Engine States",
                                }
                            }
                        ]
                    }
                }
            ]
        }
    ];
}

    