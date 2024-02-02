using MudBlazor.Services;
using Radiate.Client.Bootstrap;
using Radiate.Client.Components;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.Selectors;
using Radiate.Client.Components.Store.States.Features;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLogging(builder => builder.AddConsole())
    .AddServices()
    .AddMudServices()
    .AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Lifetime.ApplicationStarted.Register(() =>
{
    var store = app.Services.GetRequiredService<IStore>();
    store.Register(new RootFeature());
    
    store.Selctors(RunSelectors.Select);
    store.Selctors(MetricsSelectors.Select);
    store.Selctors(EngineRunStateSelector.Select);
});

app.Run();