using PokemonLib.Models;
using Refit;

namespace PokemonLib.Services;

public class PokemonService
{
    private readonly IPokemonApi _pokemonApi;
    public PokemonService()
    {
        var host = "https://pokeapi.co";
        _pokemonApi = RestService.For<IPokemonApi>(host);
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        return await _pokemonApi.GetPokemonListAsync(10000, 0);
    }
    
    public async Task<Pokemon> GetPokemonDetails(string name)
    {
        return await _pokemonApi.GetPokemonDetails(name);
    }
}