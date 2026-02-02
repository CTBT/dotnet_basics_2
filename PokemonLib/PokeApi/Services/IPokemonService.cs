using PokemonLib.Models;

namespace PokemonLib.PokeApi.Services;

public interface IPokemonService
{
    Task<PokemonList> GetPokemonListAsync();
    Task<Pokemon?> GetPokemonDetailsAsync(string name);
}