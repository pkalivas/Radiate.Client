using Radiate.Client.Domain.Store.Models;
using Radiate.Client.Domain.Store.Models.Projections;
using Radiate.Client.Domain.Store.Models.States;
using Radiate.Client.Services.Schema;
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

    public static ISelector<RootState, ImageDisplayPanelProjection> SelectImageDisplayPanelModel(string imageType) => Selectors
        .Create<RootState, RunState, ImageDisplayPanelProjection>(RunSelectors.SelectRun, run =>
            new ImageDisplayPanelProjection
            {
                RunId = run.RunId,
                IsComplete = run.IsCompleted,
                IsRunning = run.IsRunning,
                ImageType = imageType,
                Image = imageType switch
                {
                    ImageTypes.Target => run.Inputs.ImageInputs.TargetImage,
                    ImageTypes.Current => run.Outputs.ImageOutput.Image,
                    _ => new ImageEntity()
                } 
            });

}