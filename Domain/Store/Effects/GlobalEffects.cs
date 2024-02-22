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
            CreateNewRunEffect
        };

    private IEffect<RootState> CreateNewRunEffect => CreateEffect<RootState>(store => store
        .OnAction<CreateNewRunAction>()
        .Select(pair =>
        {
            var (state, action) = pair;
            
            IRunTemplate template = action.ModelType switch
            {
                ModelTypes.Graph => new GraphTemplate(),
                ModelTypes.Image => new ImageTemplate(),
                ModelTypes.Tree => new TreeTemplate(),
                ModelTypes.MultiObjective => new MultiObjectiveTemplate(),
                _ => throw new ArgumentOutOfRangeException()
            };

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
                RunTemplate = template,
                PanelExpanded = template.UI.LeftSideAccordion.ExpansionPanels
                    .Concat(template.UI.RightSideAccordion.ExpansionPanels)
                    .ToImmutableDictionary(key => key.Id, val => val.IsOpen),
            };

            return new List<IAction>
            {
                new RunCreatedAction(newRun),
                new RunUiCreatedAction(newRunUi)
            };
        }), true);

}