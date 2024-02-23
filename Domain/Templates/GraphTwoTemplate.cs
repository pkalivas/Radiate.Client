using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;

namespace Radiate.Client.Domain.Templates;


public class GraphTwoTemplate : IRunTemplate
{
    public Guid Id => new("8EACF674-1E77-434E-89D5-B5CBC17FA568");
    public string ModelType => ModelTypes.Graph;
    public IRunUITemplate UI => new GraphTwoUiTemplate();
}

public class GraphTwoUiTemplate : IRunUITemplate
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