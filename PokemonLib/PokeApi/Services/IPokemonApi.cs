using PokemonLib.Models;
using Refit;

namespace PokemonLib.PokeApi.Services;

public interface IPokemonApi
{
    [Get("/api/v2/pokemon?limit={limit}&offset={offset}")]
    Task<PokemonList> GetPokemonListAsync(int limit, int offset);

    [Get("/api/v2/pokemon/{name}")]
    public Task<ApiResponse<Pokemon>> GetPokemonDetailsAsync(string name);
}