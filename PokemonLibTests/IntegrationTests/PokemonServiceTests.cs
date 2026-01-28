using System.Net;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using PokemonLib.Models;
using PokemonLib.Services;
using Refit;

namespace PokemonLibTests;

public class PokemonServiceTests
{
    private readonly Mock<IPokemonApi> _pokemonApi = new();

    [Fact]
    public async Task GetPokemonDetails_ReturnsExpectedPokemon()
    {
        // arrange
        _pokemonApi.Setup(api => api.GetPokemonDetailsAsync("bulbasaur"))
            .ReturnsAsync(new ApiResponse<Pokemon>(
                new HttpResponseMessage(HttpStatusCode.OK),
                new Pokemon
                {
                    Name = "bulbasaur",
                    Height = 7,
                    Weight = 69,
                    Moves = new List<MoveListItem>
                    {
                        new() { Move = new Move(name: "tackle") },
                        new() { Move = new Move(name: "vine whip") }
                    }
                },
                null
            ));
        
        var pokemonService = new PokemonService(_pokemonApi.Object, NullLogger<PokemonService>.Instance);
        
        // act
        var result = await pokemonService.GetPokemonDetailsAsync("bulbasaur");

        // assert
        Assert.True(result.Name == "bulbasaur");
    }
    
    [Fact]
    public async Task GetPokemonDetails_ReturnsNullIfPokemonNotFound()
    {
        // arrange
        _pokemonApi.Setup(api => api.GetPokemonDetailsAsync("notfounditem"))
            .ReturnsAsync(() => new ApiResponse<Pokemon>(new HttpResponseMessage(HttpStatusCode.NotFound), null, null));
        
        var pokemonService = new PokemonService(_pokemonApi.Object, NullLogger<PokemonService>.Instance);
        
        // act
        var result = await pokemonService.GetPokemonDetailsAsync("notfounditem");

        // assert
        Assert.True(result is null);
    }
}