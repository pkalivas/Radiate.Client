using Radiate.Client.Components.Store.States;
using Radiate.Client.Services.Genome;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Codex;
using Radiate.Optimizers.Evolution.Genome;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Runners;

public class CircleEngineRunner : IEngineRunner
{
    public Func<CancellationToken, Task> Run(RunInput command, CancellationTokenSource cts) => async token =>
    {
        var targetImage = command.GetInputValue<string>("TargetImage");
        var numCircles = command.GetInputValue<int>("NumCircles");
        var mutationRate = command.GetInputValue<float>("MutationRate");
        var iterationLimit = command.GetInputValue<int>("IterationLimit");
        var populationSize = command.GetInputValue<int>("PopulationSize");
        
        var image = Image.Load<Rgba32>(Convert.FromBase64String(targetImage));
        
        var codex = new Codex<CircleGene, CircleChromosome>()
            .Encoder(() => Genotype.Create(new CircleChromosome(Enumerable
                .Range(0, numCircles)
                .Select(_ => new CircleGene(Circle.NewRandom()))
                .ToArray())))
            .Decoder(geno => (CircleChromosome)geno.GetChromosome());

        var engine = Engine.Genetic(codex).Async()
            .PopulationSize(populationSize)
            .Minimizing()
            .Alterers(
                new CircleMutator(mutationRate, 0.1f),
                new MeanCrossover<CircleGene>(),
                new UniformCrossover<CircleGene>(0.5f))
            .Build(geno => Fitness(geno, image));

        var result = engine.Fit()
            .Limit(Limits.Iteration(iterationLimit))
            // .Peek(res => resultCallback(Map(res, image.Height, image.Width)))
            .TakeWhile(_ => !cts.IsCancellationRequested && !token.IsCancellationRequested)
            .ToResult();
        
        // resultCallback(Map(result, 500, 500));
    };

    public RunInput GetInputs(RunInputState feature) => new()
    {
        Inputs = new List<RunInputValue>
        {
            // new("TargetImage", feature.Target.ImageDataString(), nameof(String)),
            // new("NumCircles", feature.EngineInputs.NumShapes.ToString(), nameof(Int32)),
            new("MutationRate", "0.1", nameof(Single)),
            new("IterationLimit", "1000", nameof(Int32)),
            new("PopulationSize", "100", nameof(Int32))
        }
    };
    private static RunOutputsState Map(EngineOutput<GeneticEpoch<CircleGene>, CircleChromosome> output, int height, int width)
    {
        var model = output.GetModel().Draw(width, height);
        
        return new RunOutputsState
        {
            EngineState = output.GetState(output.EngineId),
            EngineStates = output.EngineStates,
            EngineId = output.EngineId,
            Metrics = output.Metrics,
            Outputs = new List<RunOutputValue>
            {
                new("Image", model.ImageDataString(), nameof(String)),
                new("Display", output.ToString(), nameof(String))
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