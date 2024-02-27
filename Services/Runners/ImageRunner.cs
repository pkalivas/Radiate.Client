using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Genome.Chromosomes;
using Radiate.Client.Services.Genome.Genes;
using Radiate.Client.Services.Genome.Interfaces;
using Radiate.Engines.Entities;
using Radiate.Extensions;
using Radiate.Optimizers.Interfaces;
using Reflow.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Runners;

public abstract class ImageRunner<TGene, TAllele> : EngineRunner<GeneticEpoch<TGene>, ImageChromosome<TGene, TAllele>>
    where TGene : ImageGene<TGene, TAllele>
    where TAllele : IDrawable, IFactory<TAllele>
{
    private const int FinalSize = 500;
    
    protected ImageRunner(IStore<RootState> store) : base(store) { }

    protected override RunOutputsState MapOnOutput(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<TGene>, ImageChromosome<TGene, TAllele>> output) => runOutputs with
    {
        ImageOutput = new ImageOutput
        {
            Image = output.GetModel().Draw(runInputs.ImageInputs.Width, runInputs.ImageInputs.Height)
        }
    };

    protected override RunOutputsState MapOnStop(RunInputsState runInputs,
        RunOutputsState runOutputs,
        EngineOutput<GeneticEpoch<TGene>, ImageChromosome<TGene, TAllele>> output) => runOutputs with
    {
        ImageOutput = new ImageOutput
        {
            Image = output.GetModel().Draw(FinalSize, FinalSize)
        }
    };

    public static float Fitness(ImageChromosome<TGene, TAllele> chromosome, Image<Rgba32> target)
    {
        var img = chromosome.Draw(target.Width, target.Height);

        if (img.ImageData.Height != target.Height || img.ImageData.Width != target.Width)
        {
            throw new InvalidOperationException(
                $"Image sizes do not match\nTarget: {target.Width}x{target.Height}\nActual: {img.ImageData.Width}x{img.ImageData.Height}");
        }
         
        var diff = 0f;
        for (var i = 0; i < target.Width; i++)
        {
            for (var j = 0; j < target.Height; j++)
            {
                var p1 = img.ImageData[i, j];
                var p2 = target[i, j];

                var dg = p2.G - p1.G;
                var dr = p2.R - p1.R;
                var db = p2.B - p1.B;

                diff += dg * dg + dr * dr + db * db;
            }
        }

        return diff / (img.ImageData.Width * img.ImageData.Height * 3);
    }
}