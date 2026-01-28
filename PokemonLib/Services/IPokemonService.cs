using PokemonLib.Models;

namespace PokemonLib.Services;

public interface IPokemonService
{
    Task<PokemonList> GetPokemonListAsync();
    Task<Pokemon?> GetPokemonDetailsAsync(string name);
}