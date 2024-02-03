using Radiate.Client.Components.Store.States;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Services.Actors.Commands;

public interface IRunsCommand
{
    Guid RunId { get; }
}

public interface IRunsActorMessage
{
    Guid RunId { get; }
}

public record RunsActorMessage<T>(T Message) : IRunsActorMessage where T : IRunsCommand
{
    public Guid RunId => Message.RunId;
}


public record StartRunCommand(Guid RunId, RunInputsFeature Inputs) : IRunsCommand;

public record StopRunCommand(Guid RunId) : IRunsCommand;