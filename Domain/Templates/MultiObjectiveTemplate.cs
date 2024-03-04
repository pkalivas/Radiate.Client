using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Domain.Templates.Panels;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Domain.Templates;


public class MultiObjectiveTemplate : IRunTemplate
{
    public Guid Id => Guid.NewGuid();
    public string ModelType => ModelTypes.Image;
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
                    ColSpan = 7,
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
                    ColSpan = 8,
                    Panel = new BoundedPaperPanel
                    {
                        Id = new Guid("17207E0C-9149-47AD-9DFF-306F89044460"),
                        Content = typeof(MetricsDataGridPanel),
                        Title = "Metrics",
                        Height = 500
                    }
                },
                new GridPanel.GridItem
                {
                    ColSpan = 4,
                    Panel = new BoundedPaperPanel
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
