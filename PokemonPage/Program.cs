using Microsoft.EntityFrameworkCore;
using Microsoft.FluentUI.AspNetCore.Components;
using PokemonLib;
using PokemonLib.Database;
using PokemonLib.PokeApi.Services;
using PokemonPage.Components;
using Refit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddRefitClient<IPokemonApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://pokeapi.co"));

builder.Services.AddFluentUIComponents();
builder.Services.AddHttpClient();

builder.Services.AddScoped<IPokemonService, PokemonDbCacheService>();

builder.Services.AddHostedService<PokemonSyncJob>();

// sqlite db
builder.Services.AddDbContext<PokemonDbContext>(options =>
{
    options.UseSqlite("Data Source=pokemon.db");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();