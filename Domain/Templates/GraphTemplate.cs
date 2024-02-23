using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Templates;

public class GraphTemplate : IRunTemplate
{
    public Guid Id { get; }
    public string ModelType { get; }
    public IRunUITemplate UI => new GraphUITemplate();
}

public class GraphUITemplate : IRunUITemplate
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
                    Panel = new PaperPanel
                    {
                        Id = new Guid("A5F2CFCD-9BB6-4511-84C3-9AB72CA8EF75"),
                        Content = typeof(AccuracyChartPanel),
                        Title = "Accuracy",
                        Height = 300
                    }
                    // Panel = new TabPanel
                    // {
                    //     Id = new Guid("53EA2B0F-E26C-45A3-8DCF-06C742998CF8"),
                    //     Tabs =
                    //     [
                    //         new()
                    //         {
                    //             Panel = new PaperPanel
                    //             {
                    //                 Id = new Guid("A5F2CFCD-9BB6-4511-84C3-9AB72CA8EF75"),
                    //                 Content = typeof(AccuracyChartPanel),
                    //                 Title = "Accuracy",
                    //                 Height = 225
                    //             }
                    //         },
                    //         new()
                    //         {
                    //             Panel = new PaperPanel
                    //             {
                    //                 Id = new Guid("4B5E7C33-5F17-4CBE-A2E0-783EE9663693"),
                    //                 Content = typeof(ScorePanel),
                    //                 Title = "Score",
                    //                 Height = 225
                    //             }
                    //         }
                    //     ]
                    // }
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
                        Actions =
                        [
                            MenuActions.Copy,
                            MenuActions.EngineTree
                        ]
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
                                Panel = new CardPanel
                                {
                                    Id = new Guid("6A3DD486-DC80-4728-A074-A1D2D79018D9"),
                                    Content = typeof(ValidationPanel),
                                    Title = "Validation",
                                }
                            }
                        ]
                    }
                }
            ]
        }
    ];
}