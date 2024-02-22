using System.Collections.Immutable;
using System.Reactive.Linq;
using Radiate.Client.Domain.Store.Actions;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Domain.Templates;
using Radiate.Client.Services.Schema;
using Reflow.Interfaces;
using static Reflow.Effects.Effects;

namespace Radiate.Client.Domain.Store.Effects;

public class GlobalEffects : IEffectRegistry<RootState>
{
    public IEnumerable<IEffect<RootState>> CreateEffects() =>
        new List<IEffect<RootState>>
        {
            CreateNewRunEffect,
            CreateRunUiEffect
        };

    private IEffect<RootState> CreateNewRunEffect => CreateEffect<RootState>(store => store
        .OnAction<CreateNewRunAction>()
        .Select(pair =>
        {
            var (state, action) = pair;
            
            var newRun = new RunState
            {
                RunId = action.RunId,
                Index = state.Runs.Count,
                Inputs = new RunInputsState
                {
                    ModelType = action.ModelType,
                    DataSetType = action.DataSetType
                }
            };

            var newRunUi = new RunUiState
            {
                RunId = action.RunId,
                RunTemplate = action.ModelType switch
                {
                    ModelTypes.Graph => new GraphTemplate(),
                    ModelTypes.Image => new ImageTemplate(),
                    ModelTypes.Tree => new TreeTemplate(),
                    ModelTypes.MultiObjective => new MultiObjectiveTemplate(),
                    _ => throw new ArgumentOutOfRangeException()
                }
            };

            return new RunCreatedAction(newRun);
        }), true);

    private IEffect<RootState> CreateRunUiEffect => CreateEffect<RootState>(store => store
        .OnAction<RunCreatedAction>()
        .Select(pair =>
        {
            var (_, action) = pair;

            var newRunUi = new RunUiState
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
            };

            return new RunUiCreatedAction(newRunUi);
        }), true);

}