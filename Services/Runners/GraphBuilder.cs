using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Extensions;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class GraphBuilder : DataSetBuilder<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    protected GraphBuilder(ITensorFrameService tensorFrameService, IStore<RootState> store) 
        : base(tensorFrameService, store) { }
    
    protected override RunOutputsState MapOnOutput(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output) => runOutputs with
    {
        GraphOutput = new GraphOutput
        {
            Graph = output.GetModel(),
            Type = typeof(PerceptronGraph<float>).FullName
        }
    };

    protected override RunOutputsState MapOnStop(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> output) =>
        MapOnOutput(runInputs, runOutputs, output);
}