using Radiate.Client.Services.Store.Models;
using Radiate.Client.Services.Store.Models.Projections;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class ImageSelectors
{
    public static ISelector<RootState, ImageTargetCurrentDisplayPanelProjection> SelectTargetCurrentDisplayPanelModel = Selectors
        .Create<RootState, RunModel, ImageTargetCurrentDisplayPanelProjection>(RunSelectors.SelectRun, run => new ImageTargetCurrentDisplayPanelProjection
        {
            RunId = run.RunId,
            Height = run.Inputs.ImageInputs.Height,
            Width = run.Inputs.ImageInputs.Width,
            TargetImage = run.Inputs.ImageInputs.TargetImage,
            CurrentImage = run.Outputs.ImageOutput.Image
        });
}