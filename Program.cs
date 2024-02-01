using MudBlazor.Services;
using Radiate.Client.Bootstrap;
using Radiate.Client.Components;
using Radiate.Client.Components.Store.Interfaces;
using Radiate.Client.Components.Store.States;

var builder = WebApplication.CreateBuilder(args);

builder.Services
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
    store.Register(new AppState());
});

app.Run();