using PokemonLib.Services;
using PokemonWebApi.Endpoints;
using Scalar.AspNetCore;

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
}

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseHttpsRedirection();

app.MapGet("/pokemon", PokemonDataEndpoints.GetPokemonList)
    .WithName("GetPokemonList").CacheOutput();

app.MapGet("/pokemon/{name}", PokemonDataEndpoints.GetPokemon)
    .WithName("GetPokemon").CacheOutput();

app.UseOutputCache();

app.Run();