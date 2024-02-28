using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class GraphSelectors
{
    public static readonly ISelector<RootState, GraphNodeTablePanelProjection> SelectGraphNodeTablePanelModel = Selectors
        .Create<RootState, RunState, GraphNodeTablePanelProjection>(RunSelectors.SelectRun, run => new GraphNodeTablePanelProjection
        {
            RunId = run.RunId,
            Graph = run.Outputs.GraphOutput.GetGraph<float>()
        });
}