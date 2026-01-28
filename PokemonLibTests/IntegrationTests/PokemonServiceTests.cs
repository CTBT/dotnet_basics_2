using Microsoft.Extensions.Logging.Abstractions;
using PokemonLib.Services;
using Refit;

namespace PokemonLibTests;

public class PokemonServiceTests
{
    private readonly PokemonService _pokemonService;

    public PokemonServiceTests()
    {
        var pokemonApi = RestService.For<IPokemonApi>("https://pokeapi.co");
        _pokemonService = new PokemonService(pokemonApi, NullLogger<PokemonService>.Instance);
    }

    [Fact]
    public async Task GetPokemonDetails_ReturnsExpectedPokemon()
    {
        // arrange
        var pokemons = (await _pokemonService.GetPokemonListAsync()).Results;
        var expectedName = pokemons.First().Name;
        // act
        var result = await _pokemonService.GetPokemonDetailsAsync(expectedName);

        // assert
        Assert.True(result.Name == expectedName);
    }
}