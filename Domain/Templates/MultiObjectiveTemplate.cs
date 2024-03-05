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
                    Id = new Guid("24E888E2-C06F-4728-80E7-648415F577EA"),
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
                    Id = new Guid("D92AE6D6-4F87-4561-BFED-E811BC84EBFC"),
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
                    Id = new Guid("35B6A650-F3B3-476D-B576-378005BBA14C"),
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
                    Id = new Guid("7718CC03-C027-4CDF-82FC-8C12F7F4E452"),
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
                    Id = new Guid("FE87E4FB-16F5-4126-9B75-78018545C825"),
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
