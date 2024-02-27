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
using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Codex;
using Radiate.Optimizers.Evolution.Genome;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners.Image;

public class CircleRunner : ImageRunner<CircleGene, Circle>
{
    public CircleRunner(IStore<RootState> store) : base(store) { }

    protected override async Task<IEngine<GeneticEpoch<CircleGene>, ImageChromosome<CircleGene, Circle>>> BuildEngine(Guid runId, RunInputsState inputs)
    {
        var imageInput = inputs.ImageInputs;
        var populationInput = inputs.PopulationInputs;
        var iterationLimit = inputs.LimitInputs.IterationLimit;
         
        var image = imageInput.TargetImage.ImageData;

        var codex = new Codex<CircleGene, ImageChromosome<CircleGene, Circle>>()
            .Encoder(() => Genotype.Create(new ImageChromosome<CircleGene, Circle>(Enumerable
                .Range(0, imageInput.NumShapes)
                .Select(_ => new CircleGene(Circle.NewRandom()))
                .ToArray())))
            .Decoder(geno => (ImageChromosome<CircleGene, Circle>)geno.GetChromosome());

        return Engine.Genetic(codex).Async()
            .PopulationSize(populationInput.PopulationSize)
            .Minimizing()
            .Alterers(
                new CircleMutator(populationInput.MutationRate, 0.1f),
                new MeanCrossover<CircleGene>(),
                new UniformCrossover<CircleGene>(0.5f))
            .Build(geno => Fitness(geno, image))
            .Limit(Limits.Iteration(iterationLimit));
    }
}