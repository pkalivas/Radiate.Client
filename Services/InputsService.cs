using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Schema;

namespace Radiate.Client.Services;

public class InputsService
{
    public RunInputsState CreateInputs(string modelType, string dataSetType) => (modelType, dataSetType) switch
    {
        (ModelTypes.Graph, _) => new RunInputsState
        {
            ModelType = modelType,
            DataSetType = dataSetType,
            LimitInputs = new LimitInputs
            {
                IterationLimit = 1000
            },
            PopulationInputs = new PopulationInputs
            {
                PopulationSize = 100,
                MutationRate = 0.02f
            }
        },
        (ModelTypes.Tree, _) => new RunInputsState
        {
            ModelType = modelType,
            DataSetType = dataSetType,
            TreeInputs = new TreeInputs
            {
                MaxDepth = 5
            },
            LimitInputs = new LimitInputs
            {
                IterationLimit = 1000
            },
            PopulationInputs = new PopulationInputs
            {
                PopulationSize = 100,
                MutationRate = 0.01f,
                CrossoverRate = 0.1f
            }
        },
        (ModelTypes.MultiObjective, _) => new RunInputsState
        {
            ModelType = modelType,
            DataSetType = dataSetType,
            LimitInputs = new LimitInputs
            {
                IterationLimit = 2000
            },
            PopulationInputs = new PopulationInputs
            {
                PopulationSize = 200,
                MutationRate = 0.02f
            },
            MultiObjectiveInputs = new MultiObjectiveInputs
            {
                FrontMaxSize = 1100,
                FrontMinSize = 1000
            }
        },
        _ => new RunInputsState
        {
            ModelType = modelType,
            DataSetType = dataSetType
        }
    };
}