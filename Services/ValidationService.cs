using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Schema;
using Radiate.Engines.Harness;
using Radiate.Extensions.Evolution.Programs;
using Radiate.Factories.Losses;

namespace Radiate.Client.Services;

public interface IValidationService
{
    RunOutputsState Validate(Guid runId, RunOutputsState outputs);
}

public class ValidationService : IValidationService
{
    private readonly ITensorFrameService _tensorFrameService;

    public ValidationService(ITensorFrameService tensorFrameService)
    {
        _tensorFrameService = tensorFrameService;
    }
    
    public RunOutputsState Validate(Guid runId, RunOutputsState outputs)
    {
        var data = _tensorFrameService.GetTensorFrame(runId);

        if (data is null)
        {
            return outputs;
        }

        switch (outputs.ModelType)
        {
            case ModelTypes.Graph:
            {
                var graph = outputs.GraphOutput.GetGraph<float>();
                var validation = new ValidationHarness<PerceptronGraph<float>>(graph, new MeanSquaredError()).Validate(data);

                return outputs with
                {
                    ValidationOutput = new ValidationOutput
                    {
                        LossFunction = validation.LossFunction,
                        TestValidation = validation.TestValidation,
                        TrainValidation = validation.TrainValidation
                    }
                };
            }
            case ModelTypes.Tree:
            {
                var graph = outputs.TreeOutput.GetTrees<float>();
                var validation = new ValidationHarness<ExpressionTree<float>>(graph, new MeanSquaredError()).Validate(data);

                return outputs with
                {
                    ValidationOutput = new ValidationOutput
                    {
                        LossFunction = validation.LossFunction,
                        TestValidation = validation.TestValidation,
                        TrainValidation = validation.TrainValidation
                    }
                };
            }
            default:
                return outputs with { };
        }
    }
}