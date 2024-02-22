using System.Collections.Immutable;
using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Genome;
using Radiate.Client.Services.Mappers;
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

    protected override Task OnStartRun(RunInputsState inputs) { return Task.CompletedTask; }

    protected override async Task<EngineOutput<GeneticEpoch<CircleGene>, CircleChromosome>> Fit(RunInputsState inputs,
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
    
    protected override RunOutputsState MapToOutput(EngineOutput<GeneticEpoch<CircleGene>, CircleChromosome> output, 
        RunInputsState inputs,
        bool isLast = false)
    {
        var state = output.GetState(output.EngineId);
        
        return  new RunOutputsState
        {
            EngineState = state,
            EngineId = output.EngineId,
            EngineStates = output.EngineStates.ToImmutableDictionary(),
            Metrics = MetricMappers.GetMetricValues(output.Metrics).ToImmutableDictionary(key => key.Name),
            ImageOutput = new ImageOutput
            {
                Image = isLast 
                    ? output.GetModel().Draw(500, 500) 
                    : output.GetModel().Draw(inputs.ImageInputs.Width, inputs.ImageInputs.Height)
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