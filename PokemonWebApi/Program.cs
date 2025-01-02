using PokemonLib.Services;
using PokemonWebApi;
using PokemonWebApi.Endpoints;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddScoped<PokemonService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/pokemon", PokemonDataEndpoints.GetPokemonList)
    .WithName("GetPokemonList");

app.MapGet("/pokemon/{name}", PokemonDataEndpoints.GetPokemon)
    .WithName("GetPokemon");

app.Run();