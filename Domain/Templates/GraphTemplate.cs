using Radiate.Client.Components.Panels;
using Radiate.Client.Components.Panels.Charts;
using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Schema;

namespace Radiate.Client.Domain.Templates;

public class GraphTemplate : IRunTemplate
{
    public Guid Id => new("8EACF674-1E77-434E-89D5-B5CBC17FA568");
    public string ModelType => ModelTypes.Graph;
    public IRunUITemplate UI => new GraphUiTemplate();
}

public class GraphUiTemplate : IRunUITemplate
{
    public Guid Id => new("AFDB4F48-1A58-4D95-85DD-530E43105E20");

    public GridListTemplate GridList => new()
    {
        Cols = 12,
        GridPanels = new List<GridPanelTemplate>
        {
            new()
            {
                Id = new Guid("53EA2B0F-E26C-45A3-8DCF-06C742998CF8"),
                Cols = 8,
                GridPanel = new ToolbarPanel
                {
                    Name = nameof(GraphInputsPanel),
                    Title = "Graph",
                    Actions = new List<string>
                    {
                        MenuActions.Copy,
                        MenuActions.EngineTree
                    }
                }
            },
            new()
            {
                Id = new Guid("365C357D-3A47-418B-84A8-7CBE2DAE1B29"),
                Cols = 4,
                GridPanel = new PanelTemplate
                {
                    Name = nameof(RunControlPanel)
                }
            }
        }
    };

    public AccordionTemplate LeftSideAccordion => new()
    {
        ExpansionPanels =
        [
            new()
            {
                Id = new Guid("6A3DD486-DC80-4728-A074-A1D2D79018D9"),
                IsOpen = true,
                ExpansionPanel = new PanelTemplate
                {
                    Name = nameof(ValidationPanel),
                    Title = "Validation",
                }
            },

            new()
            {
                Id = new Guid("C3C00DB8-DBCB-407A-85BF-432D3591CA62"),
                IsOpen = true,
                ExpansionPanel = new PanelTemplate
                {
                    Name = nameof(MetricsDataGridPanel),
                    Title = "Metrics",
                }
            }
        ]
    };

    public AccordionTemplate RightSideAccordion { get; } = new()
    {
        ExpansionPanels =
        [
            new()
            {
                Id = new Guid("F4114F2B-4C84-455C-89D5-998964CDEB1B"),
                IsOpen = true,
                MaxHeight = 200,
                ExpansionPanel = new PanelTemplate
                {
                    Name = nameof(ScorePanel),
                    Title = "Scores",
                }
            },

            new()
            {
                Id = new Guid("D9396C96-8657-447E-863E-8ACDC2A1A1BB"),
                MaxHeight = 150,
                IsOpen = true,
                ExpansionPanel = new ChartPanelTemplate
                {
                    Name = MetricNames.FitnessDistribution,
                    ChartType = ChartTypes.Line,
                    Title = MetricNames.FitnessDistribution,
                    Height = 125
                }
            },

            new()
            {
                Id = new Guid("6BA53B93-EF5A-4E2B-ABA6-87E095854D37"),
                MaxHeight = 150,
                IsOpen = true,
                ExpansionPanel = new ChartPanelTemplate
                {
                    Name = MetricNames.AgeDistribution,
                    Title = MetricNames.AgeDistribution,
                    ChartType = ChartTypes.Bar,
                    Height = 125
                }
            },

            new()
            {
                Id = new Guid("DDD74B29-572F-41F0-883E-4A458EACF295"),
                MaxHeight = 150,
                IsOpen = true,
                ExpansionPanel = new ChartPanelTemplate
                {
                    Name = MetricNames.GenomeSizeDistribution,
                    Title = MetricNames.GenomeSizeDistribution,
                    ChartType = ChartTypes.Pie,
                    Height = 125
                }
            }
        ],
    };
}