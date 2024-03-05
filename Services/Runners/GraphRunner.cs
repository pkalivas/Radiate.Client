using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Store.Models.States.Outputs;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Extensions;
using Radiate.Extensions.Evolution.Architects.Nodes;
using Radiate.Extensions.Evolution.Programs;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public abstract class GraphRunner : DataSetRunner<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>
{
    protected GraphRunner(ITensorFrameService tensorFrameService, IStore<RootState> store) 
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

    protected IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> CreateCyclicEngine(
        List<IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> engines)
    {
        if (engines.Count == 0)
        {
            throw new ArgumentException("At least one engine is required"); 
        }

        var firstEngine = engines.First();
        var otherEngines = engines.Skip(1).ToArray();
        
        return Engine.Cyclic(firstEngine, otherEngines);
    }
    
    protected IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>> CreateConcatEngine(
        List<IEngine<GeneticEpoch<GraphGene<float>>, PerceptronGraph<float>>> engines)
    {
        if (engines.Count == 0)
        {
            throw new ArgumentException("At least one engine is required"); 
        }

        var firstEngine = engines.First();
        var otherEngines = engines.Skip(1).ToArray();
        
        return Engine.Concat(firstEngine, otherEngines);
    }

}