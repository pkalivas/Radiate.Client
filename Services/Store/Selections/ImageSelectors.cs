using Radiate.Client.Services.Store.Models;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Services.Store.Selections;

public static class ImageSelectors
{
    public static ISelector<RootState, ImageCardPanelModel> GetImageCardPanelModel = Selectors
        .Create<RootState, RunModel, ImageCardPanelModel>(RunSelectors.SelectRun, run => new ImageCardPanelModel
        {
            RunId = run.RunId,
            Height = run.Inputs.ImageInputs.Height,
            Width = run.Inputs.ImageInputs.Width,
            TargetImage = run.Inputs.ImageInputs.TargetImage,
            CurrentImage = run.Outputs.ImageOutput.Image
        });
}