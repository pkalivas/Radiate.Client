using MudBlazor.Services;
using Radiate.Client.Bootstrap;
using Radiate.Client.Components;

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

app.Run();