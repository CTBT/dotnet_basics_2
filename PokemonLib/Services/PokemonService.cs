using System.Net;
using Microsoft.Extensions.Logging;
using PokemonLib.Models;
using Refit;

namespace PokemonLib.Services;

public class PokemonService : IPokemonService
{
    private readonly ILogger<PokemonService> _logger;
    private readonly IPokemonApi _pokemonApi;
    public PokemonService(IPokemonApi pokemonApi, ILogger<PokemonService> logger)
    {
        _logger = logger;
        _pokemonApi = pokemonApi;
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        return await _pokemonApi.GetPokemonListAsync(10000, 0);
    }
    
    public async Task<Pokemon?> GetPokemonDetailsAsync(string name)
    {
        _logger.LogInformation("Requesting details for pokemon {Name} from the pokeapi", name);
        var result = await _pokemonApi.GetPokemonDetailsAsync(name);
        
        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning("Pokemon {Name} was not found in the external api", name);
            return null;
        }
        
        return result.Content;
    }
}