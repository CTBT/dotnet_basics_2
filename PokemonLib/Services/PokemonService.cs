using System.Net;
using Microsoft.Extensions.Logging;
using PokemonLib.Models;
using Refit;

namespace PokemonLib.Services;

public class PokemonService
{
    private readonly ILogger<PokemonService> _logger;
    private readonly IPokemonApi _pokemonApi;
    public PokemonService(ILogger<PokemonService> logger)
    {
        _logger = logger;
        var host = "https://pokeapi.co";
        _pokemonApi = RestService.For<IPokemonApi>(host);
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        return await _pokemonApi.GetPokemonListAsync(10000, 0);
    }
    
    public async Task<Pokemon?> GetPokemonDetails(string name)
    {
        try
        {
            _logger.LogInformation("Requesting details for pokemon {Name} from the pokeapi", name);
            return await _pokemonApi.GetPokemonDetails(name);
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.LogWarning("Pokemon {Name} was not found in the external api", name);
                return null;
            }
            throw;
        }
    }
}