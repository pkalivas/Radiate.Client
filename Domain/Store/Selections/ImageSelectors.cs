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
    
    public static ISelector<RootState, ImageHeaderProjection> SelectPanelToolbarModel(string imageType) => Selectors
        .Create<RootState, RunState, ImageHeaderProjection>(RunSelectors.SelectRun, control => new ImageHeaderProjection
        {
            RunId = control.RunId,
            IsRunning = control.IsRunning,
            IsPaused = control.IsPaused,
            IsComplete = control.IsCompleted,
            Height = control.Inputs.ImageInputs.Height,
            Width = control.Inputs.ImageInputs.Width,
            ImageType = imageType,
            Image = imageType switch
            {
                ImageTypes.Target => control.Inputs.ImageInputs.TargetImage,
                ImageTypes.Current => control.Outputs.ImageOutput.Image,
                _ => new ImageEntity()
            },
            DisplayWidth = control.Inputs.ImageInputs.DisplayWidth,
            DisplayHeight = control.Inputs.ImageInputs.DisplayHeight
        });


}