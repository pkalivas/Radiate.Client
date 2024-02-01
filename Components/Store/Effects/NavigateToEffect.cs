using Microsoft.AspNetCore.Components;
using Radiate.Client.Components.Store.Actions;
using Radiate.Client.Components.Store.States.Features;

namespace Radiate.Client.Components.Store.Effects;

public class NavigateToEffect : Effect<RootFeature, NavigateToRunAction>
{
    public NavigateToEffect(IServiceProvider serviceProvider) : base(serviceProvider) { }

    public override async Task HandleAsync(RootFeature state, NavigateToRunAction action, IDispatcher dispatcher)
    {
        await using var scope = ServiceProvider.CreateAsyncScope();
        var navigationService = scope.ServiceProvider.GetRequiredService<NavigationManager>();
        var uri = $"/runs/{action.RunId}";
        navigationService.NavigateTo(uri);
    }
}