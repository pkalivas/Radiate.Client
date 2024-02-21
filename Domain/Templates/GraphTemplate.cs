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

    public List<IPanelTemplate> Components => new()
    {
        new GridTemplate
        {
            Id = new Guid("53EA2B0F-E26C-45A3-8DCF-06C742998CF8"),
            Components = 
            {
                new GridItemPanelTemplate
                {
                    Cols = 8,
                    Id = new Guid("601F09C0-8EC9-41CC-9045-9A51F8672F70"),
                    Components =
                    [
                        new ExpansionPanelTemplate
                        {
                            Id = new Guid("E3A3E3A3-3E3A-4E3A-8E3A-3E3A3E3A3E3A"),
                            Components = new List<IPanelTemplate>
                            {
                                new ExpansionPanelItemTemplate
                                {
                                    Id = new Guid("95AB2103-EC32-4442-B870-E7C907CA192F"),
                                    IsExpanded = true,
                                    Panels = new List<IPanelTemplate>
                                    {
                                        new PanelTemplate
                                        {
                                            Id = new Guid("3F78FC25-4074-4638-8E13-BE44D516527C"),
                                            Name = nameof(GraphInputsPanel),
                                        }
                                    }     
                                }
                            },
                        }
                    ]
                }
            }
        }
    };
}