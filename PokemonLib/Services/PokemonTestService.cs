using Microsoft.Extensions.Logging;
using PokemonLib.Models;

namespace PokemonLib.Services;

public class PokemonTestService : IPokemonService
{
    private readonly ILogger<PokemonTestService> _logger;
    private readonly List<PokemonListItem> _items;

    public PokemonTestService(ILogger<PokemonTestService> logger)
    {
        _logger = logger;
        _logger.LogInformation("Using the fake PokemonService");
        _items = Enumerable.Range(1, 2000).Select(i => new PokemonListItem
        {
            Name = $"Pokemon_{i}",
            Url = $"https://pokeapi.co/api/v2/pokemon/{i}/"
        }).ToList();
    }
    
    public Task<PokemonList> GetPokemonListAsync()
    {
        return Task.FromResult(new PokemonList {Results = _items, });
    }
    
    public Task<Pokemon?> GetPokemonDetailsAsync(string name)
    {
        var item = _items.FirstOrDefault(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        return item is null ? null : Task.FromResult(new Pokemon()
        {
            Name = item?.Name,
            Height = 10,
            Weight = 100,
            Moves = new List<MoveListItem>
            {
                new() { Move = new Move(name: "Move1") }, new() {Move = new Move(name: "Move2") }
            }
        });
    }
}