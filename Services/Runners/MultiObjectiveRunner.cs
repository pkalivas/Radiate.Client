using Radiate.Client.Domain.Store;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Mappers;
using Radiate.Client.Services.Schema;
using Radiate.Engines;
using Radiate.Engines.Entities;
using Radiate.Engines.Limits;
using Radiate.Extensions;
using Radiate.Optimizers.Evolution.Alterers;
using Radiate.Optimizers.Evolution.Codex;
using Radiate.Optimizers.Evolution.Genome;
using Radiate.Optimizers.Evolution.Genome.Genes;
using Radiate.Optimizers.Evolution.MOEA;
using Radiate.Optimizers.Evolution.Selectors;
using Reflow.Interfaces;

namespace Radiate.Client.Services.Runners;

public class MultiObjectiveRunner : EngineRunner<GeneticEpoch<FloatGene>, float[]>
{
    private const int Variables = 4;
    private const int Objectives = 3;
    private const int K = Variables - Objectives + 1;

    private Front<float[]>? _front;

    public MultiObjectiveRunner(IStore<RootState> store) : base(store) { }

    protected override async Task OnStartRun(RunInputsState inputs)
    {
        _front = null;
    }

    protected override async Task<EngineOutput<GeneticEpoch<FloatGene>, float[]>> Fit(RunInputsState inputs,
        CancellationTokenSource cts, 
        Action<EngineOutput<GeneticEpoch<FloatGene>, float[]>> onEngineComplete)
    {
        var iterationLimit = inputs.LimitInputs.IterationLimit;
        var populationSize = inputs.PopulationInputs.PopulationSize;
        
        Func<float[], float[]> fitnessFunction = inputs.DataSetType switch
        {
            DataSetTypes.DTLZ1 => FitnessDTLZ1,
            DataSetTypes.DTLZ2 => FitnessDTLZ2,
            DataSetTypes.DTLZ6 => FitnessDTZL6,
            DataSetTypes.DTLZ7 => FitnessDTZL7,
            _ => throw new Exception("Invalid choice.")
        };
        
        var codex = new Codex<FloatGene, float[]>()
            .Encoder(() => Genotype.Create(Chromosome.FloatChromosome(Variables)))
            .Decoder(geno => fitnessFunction(geno.GetChromosome().Select(g => g.Allele).ToArray()));

        var engine = Engine.Genetic(codex).Async()
            .Minimizing(Objectives)
            .PopulationSize(populationSize)
            .OffspringSelector(new TournamentSelector<FloatGene>())
            .SurvivorSelector(new NSGA2Selector<FloatGene>())
            .Alterers(
                new Mutator<FloatGene>(1f / Variables),
                new SimulatedBinaryCrossover<FloatGene, float>(2.5f, 1f))
            .Build();
        
        return engine.Fit()
            .Limit(Limits.Iteration(iterationLimit))
            .Peek(onEngineComplete)
            .TakeWhile(_ => !cts.IsCancellationRequested)
            .ToResult();
    }

    protected override RunOutputsState MapToOutput(EngineOutput<GeneticEpoch<FloatGene>, float[]> output,
        RunInputsState inputs, 
        bool isLast = false)
    {
        if (_front == null)
        {
            _front = new Front<float[]>(
                (one, two) => one.SequenceEqual(two),
                (one, two) => Pareto.Dominance(one, two, output.Optimize));
        }

        _front.AddAll(output.Epoch.Population
            .Select(val => val.GetFitnessValues()));
        
        if (_front.Size() < inputs.MultiObjectiveInputs.FrontMaxSize)
        {
            return new()
            {
                EngineState = output.GetState(output.EngineId),
                EngineId = output.EngineId,
                EngineStates = output.EngineStates,
                Metrics = MetricMappers.GetMetricValues(output.Metrics).ToDictionary(key => key.Name),
                ParetoFrontOutput = new ParetoFrontOutput
                {
                    Front = _front.GetPoints()
                }
            };
        }

        if (_front.Size() >= inputs.MultiObjectiveInputs.FrontMaxSize)
        {
            _front = _front.ReplaceAll(points => Pareto.CrowdingDistance(points
                    .Select(val => val)
                    .ToArray(), output.Optimize)
                .Select((val, idx) => (val, idx))
                .OrderByDescending(val => val.val)
                .Take(inputs.MultiObjectiveInputs.FrontMinSize)
                .Select(val => points[val.idx])
                .ToList());
        }

        return new()
        {
            EngineState = output.GetState(output.EngineId),
            EngineId = output.EngineId,
            EngineStates = output.EngineStates,
            Metrics = MetricMappers.GetMetricValues(output.Metrics).ToDictionary(key => key.Name),
            ParetoFrontOutput = new ParetoFrontOutput
            {
                Front = _front.GetPoints()
            }
        };
    }
    
    public static float[] FitnessDTLZ1(float[] values)
    {
        var g = 0f;
        for (var i = Variables - K; i < Variables; i++)
        {
            g += (float) Math.Pow(values[i] - 0.5f, 2f) - (float) Math.Cos(20f * Math.PI * (values[i] - 0.5f));
        }
        
        g = 100f * (K + g);
        
        var f = new float[Objectives];
        for (var i = 0; i < Objectives; i++)
        {
            f[i] = 0.5f * (1f + g);
            for (var j = 0; j < Objectives - 1 - i; j++)
            {
                f[i] *= values[j];
            }
        
            if (i != 0)
            {
                f[i] *= 1f - values[Objectives - 1 - i];
            }
        }
        
        return f;
    }

    public static float[] FitnessDTLZ2(float[] values)
    {
        var g = 0.0f;
        for (var i = Variables - K; i < Variables; i++)
        {
            g += (float) Math.Pow(values[i] - 0.5f, 2f) - (float) Math.Cos(20f * Math.PI * (values[i] - 0.5f));
        }
        g = 100.0f * (K + g);

        var f = new float[Objectives];

        for (var i = 0; i < Objectives; i++)
        {
            f[i] = 1.0f + g;
        }

        for (var i = 0; i < Objectives; i++)
        {
            for (var j = 0; j < Objectives - (i + 1); j++)
            {
                f[i] *= (float) Math.Cos(values[j] * 0.5 * Math.PI);
            }
            if (i != 0)
            {
                var aux = Objectives - (i + 1);
                f[i] *= (float)Math.Sin(values[aux] * 0.5 * Math.PI);
            }
        }

        return f;
    }
    
    public float[] FitnessDTZL6(float[] decisionVariables)
    {
        var f = new float[Objectives];
        var k = Variables - Objectives + 1;

        var g = 0.0f;
        for (var i = Variables - k; i < Variables; i++)
        {
            g += (float) Math.Pow(decisionVariables[i], 0.1);
        }

        var theta = (float) Math.PI / (4.0 * (1.0 + g));

        for (var i = 0; i < Objectives; i++)
        {
            f[i] = (1.0f + g) * (float) Math.Cos((1.0 + 2.0 * g) * Math.PI * 0.5);
        }

        for (var i = 0; i < Objectives; i++)
        {
            for (var j = 0; j < Objectives - (i + 1); j++)
            {
                f[i] *= (float) Math.Cos(theta * decisionVariables[j] * Math.PI * 0.5);
            }
            if (i != 0)
            {
                var aux = Objectives - (i + 1);
                f[i] *= (float) Math.Sin(theta * decisionVariables[aux] * Math.PI * 0.5);
            }
        }

        return f;
    }
    
    public float[] FitnessDTZL7(float[] decisionVariables)
    {
        var g = new float[Objectives];
        var x = new float[Variables - K + 1];
        
        for (var i = Variables - K; i < Variables; i++)
        {
            x[i - (Variables - K)] = decisionVariables[i];
        }
        
        var sum = 0f;
        for (var i = 0; i < x.Length; i++)
        {
            sum += x[i];
        }
        
        var h = 0f;
        for (var i = 0; i < Objectives; i++)
        {
            g[i] = 1f + 9.0f / K * sum;
        }
        
        for (var i = 0; i < Objectives - 1; i++)
        {
            h += decisionVariables[i] / (1f + g[i]) * (float) (1f + Math.Sin(3f * Math.PI * decisionVariables[i]));
        }
        
        h = Objectives - h;
        
        var f = new float[Objectives];
        for (var i = 0; i < Objectives - 1; i++)
        {
            f[i] = decisionVariables[i];
        }
        
        f[Objectives - 1] = h * (1 + g[Objectives - 1]);

        return f;
    }
}