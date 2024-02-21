using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class ImageSelectors
{
    public static readonly ISelector<RootState, ImageTargetCurrentDisplayPanelProjection> SelectTargetCurrentDisplayPanelModel = Selectors
        .Create<RootState, RunState, ImageTargetCurrentDisplayPanelProjection>(RunSelectors.SelectRun, run => new ImageTargetCurrentDisplayPanelProjection
        {
            RunId = run.RunId,
            IsComplete = run.IsCompleted,
            IsRunning = run.IsRunning,
            TargetImage = run.Inputs.ImageInputs.TargetImage,
            CurrentImage = run.Outputs.ImageOutput.Image
        });
}