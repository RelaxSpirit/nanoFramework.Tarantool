using nanoFramework.Tarantool;
using nanoFramework.Tarantool.Client.Interfaces;
using WeatherTracker.Components;

const string TarantoolHostIp = "YourTarantoolIpAddress";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSingleton<IBox>(sp => TarantoolContext.Connect($"{TarantoolHostIp}:3301"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
