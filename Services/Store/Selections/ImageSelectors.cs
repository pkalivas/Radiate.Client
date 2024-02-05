using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class ImageSelectors
{
    public static ISelector<RootState, ImageTargetCurrentDisplayModel> SelectTargetCurrentDisplayPanelModel = Selectors
        .Create<RootState, RunModel, ImageTargetCurrentDisplayModel>(RunSelectors.SelectRun, run => new ImageTargetCurrentDisplayModel
        {
            RunId = run.RunId,
            Height = run.Inputs.ImageInputs.Height,
            Width = run.Inputs.ImageInputs.Width,
            TargetImage = run.Inputs.ImageInputs.TargetImage,
            CurrentImage = run.Outputs.ImageOutput.Image
        });
}