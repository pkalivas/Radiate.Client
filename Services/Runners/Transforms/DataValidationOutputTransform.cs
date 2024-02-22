using Radiate.Client.Domain.Store.Models.States;
using Radiate.Engines.Entities;
using Radiate.Engines.Harness;
using Radiate.Engines.Interfaces;
using Radiate.Extensions;
using Radiate.Factories.Losses;
using Radiate.Optimizers.Interfaces;
using Radiate.Tensors;

namespace Radiate.Client.Services.Runners.Transforms;

public class DataValidationOutputTransform<TEpoch, T> : IRunOutputTransform<TEpoch, T> 
    where TEpoch : IEpoch
    where T : IPredictionModel<T>
{
    private readonly ILossFunction _lossFunction;
    private readonly TensorFrame _frame;
    
    public DataValidationOutputTransform(TensorFrame frame, ILossFunction lossFunction)
    {
        _lossFunction = lossFunction;
        _frame = frame;
    }
    
    public RunOutputsState Transform(EngineOutput<TEpoch, T> handle, RunOutputsState output, RunInputsState input, bool isLast)
    {
        var validation = new ValidationHarness<T>(handle.GetModel(), _lossFunction).Validate(_frame);

        return output with
        {
            ValidationOutput = new ValidationOutput
            {
                LossFunction = validation.LossFunction,
                TrainValidation = validation.TrainValidation,
                TestValidation = validation.TestValidation
            }
        };
    }
}