using Radiate.Client.Domain.Store.Models;
using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Schema;
using Reflow.Interfaces;
using Reflow.Selectors;

namespace Radiate.Client.Domain.Store.Selections;

public static class ImageSelectors
{
    public static ISelector<RootState, ImageDisplayPanelProjection> SelectImageDisplayPanelModel(string imageType) => Selectors
        .Create<RootState, RunState, ImageDisplayPanelProjection>(RunSelectors.SelectRun, run =>
            new ImageDisplayPanelProjection
            {
                RunId = run.RunId,
                IsComplete = run.IsCompleted,
                IsRunning = run.IsRunning,
                IsPaused = run.IsPaused,
                ImageType = imageType,
                Width = run.Inputs.ImageInputs.Width,
                Height = run.Inputs.ImageInputs.Height,
                DisplayWidth = run.Inputs.ImageInputs.DisplayWidth,
                DisplayHeight = run.Inputs.ImageInputs.DisplayHeight,
                Image = imageType switch
                {
                    ImageTypes.Target => run.Inputs.ImageInputs.TargetImage,
                    ImageTypes.Current => run.Outputs.ImageOutput.Image,
                    _ => new ImageEntity()
                } 
            });
}