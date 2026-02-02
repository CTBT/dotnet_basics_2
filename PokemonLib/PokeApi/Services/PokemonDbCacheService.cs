using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PokemonLib.Database;
using PokemonLib.Models;

namespace PokemonLib.PokeApi.Services;

public class PokemonDbCacheService : IPokemonService
{
    private readonly ILogger<PokemonDbCacheService> _logger;
    private readonly PokemonDbContext _context;
    private readonly IPokemonApi _pokemonApi;
    public PokemonDbCacheService(IPokemonApi pokemonApi, ILogger<PokemonDbCacheService> logger, PokemonDbContext context)
    {
        _logger = logger;
        _context = context;
        _context.Database.EnsureCreated();
        _pokemonApi = pokemonApi;
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        return await _pokemonApi.GetPokemonListAsync(10000, 0);
    }
    
    public async Task<Pokemon?> GetPokemonDetailsAsync(string name)
    {
        var dbItem = await _context.Pokemons
            .Include(i => i.Moves)
            .SingleOrDefaultAsync(p => p.Name == name);

        if (dbItem is not null)
        {
            return new Pokemon()
            {
                Id =  dbItem.Id,
                Name = dbItem.Name,
                Height = dbItem.Height,
                Weight = dbItem.Weight,
                Moves = dbItem.Moves.Select(i => new MoveListItem() { Move = new Move(i.Name) }).ToList()
            };
        }

        _logger.LogDebug("Requesting details for pokemon {Name} from the pokeapi", name);
        var result = await _pokemonApi.GetPokemonDetailsAsync(name);
            
        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            _logger.LogWarning("Pokemon {Name} was not found in the external api", name);
            return null;
        }
            
        var dbPokemon = new DbPokemon()
        {
            Id = result.Content.Id,
            Name = result.Content.Name,
            Height = result.Content.Height,
            Weight = result.Content.Weight,
            Moves = result.Content.Moves.Select(m => new DbMove() { Name = m.Move.Name }).ToList()
        };
        _context.Pokemons.Add(dbPokemon);
        await _context.SaveChangesAsync();
        
        return result.Content;

    }
}