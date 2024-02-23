using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Extensions;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Services.Runners.OutputTransforms;

public class GraphOutputTransform : IRunOutputTransform<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public RunOutputsState Transform(Guid runId, EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> handle,
        RunOutputsState output,
        RunInputsState input,
        bool isLast) => output with
        {
            GraphOutput = new GraphOutput
            {
                Type = typeof(PerceptronGraph<float>).FullName,
                Graph = handle.GetModel()
            }
        };
}