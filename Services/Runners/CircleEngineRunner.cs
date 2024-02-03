using Radiate.Client.Services.Genome;
using Radiate.Client.Services.Runners.Interfaces;
using Radiate.Client.Services.Store;
using Radiate.Client.Services.Store.Models;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Codex;
using Radiate.Optimizers.Evolution.Genome;
using Reflow.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Runners;

public class CircleEngineRunner : EngineRunner<GeneticEpoch<CircleGene>, CircleChromosome>
{
    public CircleEngineRunner(IStore<RootState> store) : base(store) { }

    protected override async Task<EngineOutput<GeneticEpoch<CircleGene>, CircleChromosome>> Fit(RunInputsModel inputs,
        CancellationTokenSource cts,
        Action<EngineOutput<GeneticEpoch<CircleGene>, CircleChromosome>> onEngineComplete)
    {
        var imageInput = inputs.ImageInputs;
        var populationInput = inputs.PopulationInputs;
        var iterationLimit = inputs.LimitInputs.IterationLimit;
        
        var image = imageInput.TargetImage.ImageData;

        var codex = new Codex<CircleGene, CircleChromosome>()
            .Encoder(() => Genotype.Create(new CircleChromosome(Enumerable
                .Range(0, imageInput.NumShapes)
                .Select(_ => new CircleGene(Circle.NewRandom()))
                .ToArray())))
            .Decoder(geno => (CircleChromosome)geno.GetChromosome());

        var engine = Engine.Genetic(codex).Async()
            .PopulationSize(populationInput.PopulationSize)
            .Minimizing()
            .Alterers(
                new CircleMutator(populationInput.MutationRate, 0.1f),
                new MeanCrossover<CircleGene>(),
                new UniformCrossover<CircleGene>(0.5f))
            .Build(geno => Fitness(geno, image));
        
        return engine.Fit()
            .Limit(Limits.Iteration(iterationLimit))
            .Peek(onEngineComplete)
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
    }
    
    protected override RunOutputsModel MapToOutput(EngineOutput<GeneticEpoch<CircleGene>, CircleChromosome> output)
    {
        var state = output.GetState(output.EngineId);
        var model = output.GetModel().Draw(500, 500);
        
        return  new RunOutputsModel
        {
            EngineState = state,
            EngineId = output.EngineId,
            EngineStates = output.EngineStates,
            Metrics = output.Metrics,
            ImageOutput = new ImageOutput
            {
                Image = model
            }
        };
    }
    
    public static float Fitness(CircleChromosome chromosome, Image<Rgba32> target)
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