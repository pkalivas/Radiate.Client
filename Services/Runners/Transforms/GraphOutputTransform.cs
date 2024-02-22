using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Extensions;
using Radiate.Extensions.Evolution.Architects.Groups;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;

namespace Radiate.Client.Services.Runners.Transforms;

public class GraphOutputTransform : IRunOutputTransform<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    public RunOutputsState Transform(EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> handle,
        RunOutputsState output,
        RunInputsState input,
        bool isLast) => output with
        {
            GraphOutput = new GraphOutput
            {
                Type = typeof(Graph<float>).FullName,
                Graph = handle.GetModel().Graph
            }
        };
}