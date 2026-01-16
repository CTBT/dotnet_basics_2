using PokemonLib.Services;
using PokemonWebApi.Endpoints;
using Refit;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddScoped<PokemonService>();
builder.Services.AddRefitClient<IPokemonApi>().ConfigureHttpClient(c =>
{
    c.BaseAddress = new Uri("https://pokeapi.co");
});
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