using Microsoft.Extensions.Logging.Abstractions;
using PokemonLib.Services;

namespace PokemonLibTests;

public class PokemonServiceTests
{
    private readonly PokemonService _pokemonService;

    public PokemonServiceTests()
    {
        _pokemonService = new PokemonService(NullLogger<PokemonService>.Instance);
    }

    [Fact]
    public async Task GetPokemonListAsync_ReturnsPokemonList()
    {
        var result = await _pokemonService.GetPokemonListAsync();

        Assert.True(result.Results.Count > 0);
    }
}