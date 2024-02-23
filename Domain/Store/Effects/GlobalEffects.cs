using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;
using Radiate.Client.Services;
using Radiate.Client.Services.Schema;
using Reflow.Interfaces;
using static Reflow.Effects.Effects;

namespace Radiate.Client.Domain.Store.Effects;

public class GlobalEffects : IEffectRegistry<RootState>
{
    private readonly InputsService _inputsService;

    public GlobalEffects(InputsService inputsService)
    {
        _inputsService = inputsService;
    }
    
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            CreateNewRunEffect,
            CreateRunUiEffect,
            CopyRunEffect
        };

    private IEffect<RootState> CopyRunEffect => CreateEffect<RootState>(store => store
        .OnAction<CopyRunAction>()
        .Select(pair =>
        {
            var (state, action) = pair;
            var (currentRunId, newRunId) = action;

            var currentRun = state.Runs[currentRunId];
            
            return new RunCreatedAction(new RunState
            {
                RunId = newRunId,
                Index = state.Runs.Count,
                Inputs = currentRun.Inputs with { }
            });
        }), true);

    private IEffect<RootState> CreateNewRunEffect => CreateEffect<RootState>(store => store
        .OnAction<CreateNewRunAction>()
        .Select(pair =>
        {
            var (state, action) = pair;

            return new RunCreatedAction(new RunState
            {
                RunId = action.RunId,
                Index = state.Runs.Count,
                Inputs = _inputsService.CreateInputs(action.ModelType, action.DataSetType)
            });
        }), true);

    private IEffect<RootState> CreateRunUiEffect => CreateEffect<RootState>(store => store
        .OnAction<RunCreatedAction>()
        .Select(pair =>
        {
            var (_, action) = pair;

            return new RunUiCreatedAction(new RunUiState
            {
                RunId = action.Run.RunId,
                RunTemplate = action.Run.Inputs.ModelType switch
                {
                    ModelTypes.Graph => new GraphTemplate(),
                    ModelTypes.Image => new ImageTemplate(),
                    ModelTypes.Tree => new TreeTemplate(),
                    ModelTypes.MultiObjective => new MultiObjectiveTemplate(),
                    _ => throw new ArgumentOutOfRangeException()
                }
            });
        }), true);

}