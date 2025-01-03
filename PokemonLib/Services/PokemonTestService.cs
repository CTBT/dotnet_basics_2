using Microsoft.Extensions.Logging;
using PokemonLib.Models;

namespace PokemonLib.Services;

public class PokemonTestService : IPokemonService
{
    private readonly ILogger<PokemonTestService> _logger;
    public PokemonTestService(ILogger<PokemonTestService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Using the fake PokemonService");
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        return new PokemonList()
        {
            Results = Enumerable.Range(1, 2000).Select(i => new PokemonListItem
            {
                Name = $"Pokemon_{i}",
                Url = $"https://pokeapi.co/api/v2/pokemon/{i}/"
            }).ToList()
        };
    }
    
    public async Task<Pokemon?> GetPokemonDetails(string name)
    {
        return new Pokemon()
        {
            Name = name,
            Height = 10,
            Weight = 100,
            Moves = new List<MoveListItem>
            {
                new() { Move = new Move(name: "Move1") }, new() {Move = new Move(name: "Move2") }
            }
        };
    }
}