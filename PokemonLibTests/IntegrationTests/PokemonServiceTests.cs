using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PokemonLib.Models;
using PokemonLib.Services;

namespace PokemonLibTests.IntegrationTests;

public class PokemonServiceTests
{
    private readonly PokemonService _pokemonService;
    private readonly Mock<IPokemonApi> _mockPokemonApi;

    public PokemonServiceTests()
    {
        _mockPokemonApi = new Mock<IPokemonApi>();
        _pokemonService = new PokemonService(_mockPokemonApi.Object, NullLogger<PokemonService>.Instance);
    }

    [Fact]
    public async Task GetPokemonDetails_ReturnsExpectedPokemon()
    {
        // arrange
        var expectedName = "pikachu";
        
        var mockPokemon = new Pokemon
        {
            Name = expectedName,
            Id = 25,
            Height = 4,
            Weight = 60,
            Moves = new List<MoveListItem>()
        };
        
        _mockPokemonApi.Setup(api => api.GetPokemonDetails(expectedName))
            .ReturnsAsync(mockPokemon);
        // act
        var result = await _pokemonService.GetPokemonDetails(expectedName);

        // assert
        Assert.NotNull(result);
        Assert.Equal(expectedName, result.Name);
    }
}