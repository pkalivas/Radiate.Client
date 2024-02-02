using Radiate.Client.Components.Store.States;
using Radiate.Client.Services.Genome;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Codex;
using Radiate.Optimizers.Evolution.Genome;
using Radiate.Schema;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace Radiate.Client.Services.Runners;

public class PolygonEngineRunner : IEngineRunner
{
    public Func<CancellationToken, Task> Run(RunInput command, CancellationTokenSource cts) => async token =>
    {
        var targetImage = command.GetInputValue<string>("TargetImage");
        var polygonCount = command.GetInputValue<int>("PolygonCount");
        var polygonLength = command.GetInputValue<int>("PolygonLength");
        var mutationRate = command.GetInputValue<float>("MutationRate");
        var iterationLimit = command.GetInputValue<int>("IterationLimit");
        var populationSize = command.GetInputValue<int>("PopulationSize");
        
        var image = Image.Load<Rgba32>(Convert.FromBase64String(targetImage));

        var random = RandomRegistry.GetRandom();
        var codex = new Codex<PolygonGene, PolygonChromosome>()
            .Encoder(() => Genotype.Create(new PolygonChromosome(Enumerable
                .Range(0, polygonCount)
                .Select(_ => new PolygonGene(Polygon.NewRandom(polygonLength, random)))
                .ToArray())))
            .Decoder(geno => (PolygonChromosome)geno.GetChromosome());

        var engine = Engine.Genetic(codex).Async()
            .PopulationSize(populationSize)
            .Minimizing()
            .Alterers(
                new PolygonMutator(mutationRate, 0.1f),
                new MeanCrossover<PolygonGene>(),
                new UniformCrossover<PolygonGene>(0.5f))
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
            // new("TargetImage", feature.ImageState.Target.ImageDataString(), nameof(String)),
            // new("PolygonCount", feature.EngineInputs.NumShapes.ToString(), nameof(Int32)),
            // new("PolygonLength", feature.EngineInputs.NumVertices.ToString(), nameof(Int32)),
            // new("PopulationSize", feature.EngineInputs.PopulationSize.ToString(), nameof(Int32)),
            // new("MutationRate", feature.EngineInputs.MutationRate.ToString(), nameof(Single)),
            // new("IterationLimit", feature.EngineInputs.IterationLimit.ToString(), nameof(Int32)),
        }
    };

    private static RunOutputsState Map(EngineOutput<GeneticEpoch<PolygonGene>, PolygonChromosome> output, int height, int width)
    {
        var state = output.GetState(output.EngineId);
        var model = output.GetModel().Draw(width, height);
        
        var result = new RunOutputsState
        {
            EngineState = state,
            EngineId = output.EngineId,
            EngineStates = output.EngineStates,
            Metrics = output.Metrics,
            Outputs = new List<RunOutputValue>
            {
                new("Image", model.ImageDataString(), nameof(String)),
                new("Display", output.ToString(), nameof(String))
            }
        };
        
        return result;
    }
    
    
    public static float Fitness(PolygonChromosome chromosome, Image<Rgba32> target)
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