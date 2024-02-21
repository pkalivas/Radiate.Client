using Radiate.Client.Components.Panels.Inputs;
using Radiate.Client.Domain.Store.Schema;

namespace Radiate.Client.Domain.Templates;

public class GraphTemplate : IRunTemplate
{
    public Guid Id => Guid.NewGuid();
    public string ModelType => ModelTypes.Graph;
    public IRunUITemplate UI => new GraphUiTemplate();
}

public class GraphUiTemplate : IRunUITemplate
{
    public Guid Id => new("AFDB4F48-1A58-4D95-85DD-530E43105E20");
    public GridListTemplate GridList { get; }

    public AccordionTemplate LeftSideAccordion => new()
    {
        ExpansionPanels = new List<ExpansionPanelTemplate>
        {
            new()
            {
                Id = new Guid("53EA2B0F-E26C-45A3-8DCF-06C742998CF8"),
                IsOpen = true,
                Title = "Inputs",
                ExpansionPanel = new PanelTemplate
                {
                    Name = nameof(GraphInputsPanel),
                }
            }
        }
    };

    public AccordionTemplate RightSideAccordion { get; } = new();
}