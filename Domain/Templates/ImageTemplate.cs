using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Actions;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Headers;
using Radiate.Client.Components.Panels.Images;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Components.Panels.Shared;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;

namespace Radiate.Client.Domain.Templates;


public class ImageTemplate : IRunTemplate
{
    public Guid Id => Guid.NewGuid();
    public string ModelType => ModelTypes.Image;
    public IRunUITemplate UI => new ImageUiTemplate();
}

public class ImageUiTemplate : IRunUITemplate
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
                    Id = new Guid("CCB59EA0-9313-4909-AD08-9B341395AAC5"),
                    ColSpan = 6,
                    Panel = new AccordionPanel
                    {
                        Id = new Guid("8FCA10DB-1073-494C-BF90-57847A736449"),
                        Toolbar = typeof(ToolBar),
                        Items =
                        [
                            new AccordionPanelItem
                            {
                                Id = new Guid("BE9001BD-ADD4-4BBA-82B9-5EB46DAC84A6"),
                                Content = typeof(ImageInputsPanel),
                                Title = "Image",
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
                    Id = new Guid("BBE31A85-C234-4AA9-A2EC-D2390F6F9E84"),
                    ColSpan = 6,
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
                                Id = new Guid("1B99E6FC-DBC2-473E-AEEC-BC0B71FAA25E"),
                                ColSpan = 6,
                                Panel = new CardPanel
                                {
                                    Id = new Guid("F53A706E-A837-4DDD-81AD-CA32D1A9C053"),
                                    Title = ImageTypes.Target,
                                    Header = typeof(ImageHeader),
                                    Content = typeof(ImageTargetCurrentDisplayPanel),
                                    Actions = typeof(ImageActions),
                                    Props = new Dictionary<string, object>
                                    {
                                        ["ImageType"] = ImageTypes.Target
                                    },
                                    ActionsProps = new Dictionary<string, object>
                                    {
                                        ["ImageType"] = ImageTypes.Target
                                    },
                                    HeaderProps = new Dictionary<string, object>
                                    {
                                        ["ImageType"] = ImageTypes.Target
                                    }
                                }
                            },
                            new GridPanel.GridItem
                            {
                                Id = new Guid("E1816C71-8BF5-432A-B71D-71B3102E05AB"),
                                ColSpan = 6,
                                Panel = new CardPanel
                                {
                                    Id = new Guid("69DECA60-B9C5-45C1-AB22-91C7236B2573"),
                                    Title = ImageTypes.Current,
                                    Header = typeof(ImageHeader),
                                    Content = typeof(ImageTargetCurrentDisplayPanel),
                                    Actions = typeof(ImageActions),
                                    Props = new Dictionary<string, object>
                                    {
                                        ["ImageType"] = ImageTypes.Current
                                    },
                                    ActionsProps = new Dictionary<string, object>
                                    {
                                        ["ImageType"] = ImageTypes.Current
                                    },
                                    HeaderProps = new Dictionary<string, object>
                                    {
                                        ["ImageType"] = ImageTypes.Current
                                    }
                                }
                            }
                        ]
                    },
                },
            ]
        }
    ];

    // public List<IPanel> Panels =>
    // [
    //     new GridPanel
    //     {
    //         Id = new Guid("496D83AB-3660-45B8-B9D6-2C8A23B66B12"),
    //         Items =
    //         [
    //             new GridPanel.GridItem
    //             {
    //                 Id = new Guid("CCB59EA0-9313-4909-AD08-9B341395AAC5"),
    //                 ColSpan = 12,
    //                 Panel = new ToolbarPanel
    //                 {
    //                     Id = new Guid("8FCA10DB-1073-494C-BF90-57847A736449"),
    //                     Content = typeof(ImageInputsPanel),
    //                     Title = "Image",
    //                     Actions =
    //                     [
    //                         MenuActions.Copy,
    //                         MenuActions.EngineTree
    //                     ]
    //                 }
    //             },
    //             new GridPanel.GridItem
    //             {
    //                 Id = new Guid("497AB58C-22F0-481A-92EF-57A91A05CD19"),
    //                 ColSpan = 7,
    //                 Panel = new BoundedPaperPanel
    //                 {
    //                     Id = new Guid("621B1B13-671E-4EF0-86C4-D7172BFC49D5"),
    //                     Content = typeof(ImageTargetCurrentDisplayPanel),
    //                     Title = "Images",
    //                     Height = 300
    //                 }
    //             },
    //             new GridPanel.GridItem
    //             {
    //                 Id = new Guid("B4EEF23B-56AA-4E13-8C8F-3A0B55DDBD8B"),
    //                 ColSpan = 3,
    //                 Panel = new CardPanel
    //                 {
    //                     Id = new Guid("B3234C56-9806-4E4E-ABF2-2471D90B5D91"),
    //                     Content = typeof(RunSimpleStatsPanel),
    //                     Title = "Stats",
    //                 }
    //             },
    //             new GridPanel.GridItem
    //             {
    //                 Id = new Guid("AA68E870-1D32-4C68-8321-599BE00FFF5F"),
    //                 ColSpan = 2,
    //                 Panel = new CardPanel
    //                 {
    //                     Id = new Guid("365C357D-3A47-418B-84A8-7CBE2DAE1B29"),
    //                     Content = typeof(RunControlPanel),
    //                     Title = "Control",
    //                 }
    //             },
    //             new GridPanel.GridItem
    //             {
    //                 Id = new Guid("5ACEE98E-7C98-4B4F-8ABF-000E1FC60B5F"),
    //                 ColSpan = 8,
    //                 Panel = new CardPanel
    //                 {
    //                     Id = new Guid("17207E0C-9149-47AD-9DFF-306F89044460"),
    //                     Content = typeof(MetricsDataGridPanel),
    //                     Title = "Metrics",
    //                 }
    //             },
    //             new GridPanel.GridItem
    //             {
    //                 Id = new Guid("1CECAA7A-FBD1-4CD8-A47C-EDFD87D72EDC"),
    //                 ColSpan = 4,
    //                 Panel = new GridPanel
    //                 {
    //                     Id = new Guid("6A3DD486-DC80-4728-A074-A1D2D79018D9"),
    //                     Items =
    //                     [
    //                         new GridPanel.GridItem
    //                         {
    //                             Id = new Guid("73BE7336-EB25-455A-BABF-241E2A3D18AD"),
    //                             ColSpan = 12,
    //                             Panel = new TabPanel
    //                             {
    //                                 Id = new Guid("9867301E-CD35-40B4-A411-4E9589CDB7DD"),
    //                                 ChildPanels = 
    //                                 [
    //                                      new PaperPanel
    //                                     {
    //                                         Id = new Guid("19687ECB-52DD-4D4A-B260-3EB55FB39B93"),
    //                                         Title = "Fitness Dist.",
    //                                         Content = typeof(MetricSummaryChartPanel),
    //                                         Props = new Dictionary<string, object>
    //                                         {
    //                                             ["MetricName"] = MetricNames.FitnessDistribution,
    //                                             ["ChartHeight"] = 200
    //                                         }
    //                                     },
    //                                     new PaperPanel
    //                                     {
    //                                         Id = new Guid("89DBD37A-4D39-4CD0-987E-0768C2C916C8"),
    //                                         Title = "Age Dist.",
    //                                         Content = typeof(MetricSummaryChartPanel),
    //                                         Props = new Dictionary<string, object>
    //                                         {
    //                                             ["MetricName"] = MetricNames.AgeDistribution,
    //                                             ["ChartHeight"] = 200
    //                                         }
    //                                     }
    //                                 ]
    //                             }
    //                         }
    //                     ]
    //                 }
    //             }
    //         ]
    //     }
    // ];
}
