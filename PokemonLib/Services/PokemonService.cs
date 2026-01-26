using PokemonLib.Models;

namespace PokemonLib.Services;

public class PokemonService
{
    private readonly IPokemonApi _pokemonApi;
    public PokemonService(IPokemonApi pokemonApi)
    {
        _pokemonApi = pokemonApi;
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        return await _pokemonApi.GetPokemonListAsync(10000, 0);
    }
    
    public async Task<Pokemon> GetPokemonDetailsAsync(string name)
    {
        return await _pokemonApi.GetPokemonDetailsAsync(name);
    }
}