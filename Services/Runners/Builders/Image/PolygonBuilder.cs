using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Genome;
using Radiate.Client.Services.Genome.Alterers;
using Radiate.Client.Services.Genome.Chromosomes;
using Radiate.Client.Services.Genome.Genes;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Interfaces;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Codex;
using Radiate.Optimizers.Evolution.Genome;
using Radiate.Schema;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.Builders.Image;

public class PolygonBuilder : ImageBuilder<PolygonGene, Polygon>
{
    public PolygonBuilder(IStore<RootState> store) : base(store) { }

    protected override async Task<IEngine<GeneticEpoch<PolygonGene>, ImageChromosome<PolygonGene, Polygon>>> BuildEngine(Guid runId, RunInputsState inputs)
    {
        var imageInput = inputs.ImageInputs;
        var populationInput = inputs.PopulationInputs;
        var iterationLimit = inputs.LimitInputs.IterationLimit;
         
        var image = imageInput.TargetImage.ImageData;

        var random = RandomRegistry.GetRandom();
        var codex = new Codex<PolygonGene, ImageChromosome<PolygonGene, Polygon>>()
            .Encoder(() => Genotype.Create(new ImageChromosome<PolygonGene, Polygon>(Enumerable
                .Range(0, imageInput.NumShapes)
                .Select(_ => new PolygonGene(Polygon.NewRandom(imageInput.NumVertices, random)))
                .ToArray())))
            .Decoder(geno => (ImageChromosome<PolygonGene, Polygon>)geno.GetChromosome());

        return Engine.Genetic(codex).Async()
            .PopulationSize(populationInput.PopulationSize)
            .Minimizing()
            .Alterers(
                new PolygonMutator(populationInput.MutationRate, 0.1f),
                new MeanCrossover<PolygonGene>(),
                new UniformCrossover<PolygonGene>(0.5f))
            .Build(geno => Fitness(geno, image))
            .Limit(Limits.Iteration(iterationLimit));
    }
}