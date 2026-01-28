using System.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PokemonLib.Database;
using PokemonLib.Models;
using Refit;

namespace PokemonLib.Services;

public class PokemonDbCacheService : IPokemonService
{
    private readonly ILogger<PokemonService> _logger;
    private readonly PokemonDbContext _pokemonDbContext;
    private readonly IPokemonApi _pokemonApi;
    public PokemonDbCacheService(IPokemonApi pokemonApi, ILogger<PokemonService> logger, PokemonDbContext pokemonDbContext)
    {
        _logger = logger;
        _pokemonDbContext = pokemonDbContext;
        _pokemonDbContext.Database.EnsureCreated();
        _pokemonApi = pokemonApi;
    }
    
    public async Task<PokemonList> GetPokemonListAsync()
    {
        return await _pokemonApi.GetPokemonListAsync(10000, 0);
    }
    
    public async Task<Pokemon?> GetPokemonDetailsAsync(string name)
    {
        if (_pokemonDbContext.Pokemons.Any(p => p.Name == name))
        {
            var dbItem = _pokemonDbContext.Pokemons
                .Include(i => i.Moves)
                .First(p => p.Name == name);
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
        _pokemonDbContext.Pokemons.Add(dbPokemon);
        await _pokemonDbContext.SaveChangesAsync();
        return result.Content;

    }
}