using PokemonLib.Models;
using Refit;

namespace PokemonLib.Services;

public interface IPokemonApi
{
    [Get("/api/v2/pokemon?limit={limit}&offset={offset}")]
    Task<PokemonList> GetPokemonListAsync(int limit, int offset);

    [Get("/api/v2/pokemon/{name}")]
    public Task<Pokemon> GetPokemonDetailsAsync(string name);
}