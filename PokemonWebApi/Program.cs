using System.Runtime.Intrinsics;
using PokemonLib.PokeApi.Services;
using PokemonWebApi.Endpoints;
using Scalar.AspNetCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddOutputCache();

var useTestData = builder.Configuration.GetValue<bool>("ServiceOptions:UseTestData");
if (useTestData)
{
    builder.Services.AddScoped<IPokemonService, PokemonTestService>();
}
else
{
    builder.Services.AddScoped<IPokemonService, PokemonService>();
    builder.Services.AddRefitClient<IPokemonApi>().ConfigureHttpClient(c =>
    {
        c.BaseAddress = new Uri("https://pokeapi.co");
    });
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi(); // /openapi/v1.json
    app.MapScalarApiReference(); // /scalar/v1.json
}
app.UseHttpsRedirection();

app.MapGet("/pokemon", PokemonDataEndpoints.GetPokemonList)
    .WithName("GetPokemonList").CacheOutput();

app.MapGet("/pokemon/{name}", PokemonDataEndpoints.GetPokemon)
    .WithName("GetPokemon").CacheOutput();

app.UseOutputCache();

app.Run();