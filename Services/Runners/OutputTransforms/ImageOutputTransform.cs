using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Genome.Chromosomes;
using Radiate.Client.Services.Genome.Genes;
using Radiate.Engines.Entities;
using Radiate.Extensions;

namespace Radiate.Client.Services.Runners.OutputTransforms;

public class ImageOutputTransform : IRunOutputTransform<GeneticEpoch<CircleGene>, CircleChromosome>,
    IRunOutputTransform<GeneticEpoch<PolygonGene>, PolygonChromosome>
{
    public RunOutputsState Transform(Guid runId, EngineOutput<GeneticEpoch<CircleGene>, CircleChromosome> handle,
        RunOutputsState output, RunInputsState input, bool isLast) => output with
    {
        ImageOutput = new ImageOutput
        {
            Image = isLast 
                ? handle.GetModel().Draw(500, 500)
                : handle.GetModel().Draw(input.ImageInputs.Width, input.ImageInputs.Height)
        }
    };

    public RunOutputsState Transform(Guid runId, EngineOutput<GeneticEpoch<PolygonGene>, PolygonChromosome> handle, RunOutputsState output, RunInputsState input, bool isLast) => output with
    {
        ImageOutput = new ImageOutput
        {
            Image = isLast 
                ? handle.GetModel().Draw(500, 500)
                : handle.GetModel().Draw(input.ImageInputs.Width, input.ImageInputs.Height)
        }
    };
}