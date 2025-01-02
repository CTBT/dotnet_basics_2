using PokemonConsole.Models;
using Refit;

namespace PokemonConsole.Services;

public interface IPokemonApi
{
    [Get("/api/v2/pokemon?limit=0&offset=0")]
    Task<PokemonList> GetPokemonListAsync();
}