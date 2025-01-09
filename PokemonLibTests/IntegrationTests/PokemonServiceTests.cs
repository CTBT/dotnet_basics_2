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
    public async Task GetPokemonDetails_ReturnsExpectedPokemon()
    {
        // arrange
        var pokemons = (await _pokemonService.GetPokemonListAsync()).Results;
        var expectedName = pokemons.First().Name;
        // act
        var result = await _pokemonService.GetPokemonDetails(expectedName);

        // assert
        Assert.True(result.Name == expectedName);
    }
}