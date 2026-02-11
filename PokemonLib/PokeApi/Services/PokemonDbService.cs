using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PokemonLib.Database;
using PokemonLib.PokeApi.Models;

namespace PokemonLib.PokeApi.Services;

public class PokemonDbService : IPokemonService
{
    private readonly ILogger<PokemonDbService> _logger;
    private readonly PokemonDbContext _context;
    public PokemonDbService(ILogger<PokemonDbService> logger, PokemonDbContext context)
    {
        _logger = logger;
        _context = context;
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        var pokemonListItems = await _context.Pokemons
            .Select(i => new PokemonListItem
            {
                Name = i.Name,
                Url = $"https://pokeapi.co/api/v2/pokemon/{i.Id}/"
            })
            .ToListAsync();
        
        return new PokemonList{Results = pokemonListItems};
    }
    
    public async Task<Pokemon?> GetPokemonDetailsAsync(string name)
    {
        var dbItem = await _context.Pokemons
            .Include(i => i.Moves)
            .SingleOrDefaultAsync(p => p.Name == name);

        if (dbItem is not null)
        {
            return new Pokemon
            {
                Id =  dbItem.Id,
                Name = dbItem.Name,
                Height = dbItem.Height,
                Weight = dbItem.Weight,
                Moves = dbItem.Moves.Select(i => new MoveListItem { Move = new Move{Name = i.Name} }).ToList()
            };
        }

        _logger.LogWarning("Pokemon {Name} was not found in the database", name);
        return null;
    }
}