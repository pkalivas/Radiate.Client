using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.Models;
using Radiate.Client.Components.Store.States;

namespace Radiate.Client.Components.Store.Actions;

public record RunCreatedAction(RunModel Run) : IAction<RootState>;