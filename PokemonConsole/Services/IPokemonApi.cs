using PokemonConsole.Models;
using Refit;

namespace PokemonConsole.Services;

public interface IPokemonApi
{
    [Get("/api/v2/pokemon?limit=10000&offset=0")]
    Task<PokemonList> GetPokemonListAsync();

    [Get("/api/v2/pokemon/{name}")]
    public Task<Pokemon> GetPokemonDetailsAsync(string name);
}