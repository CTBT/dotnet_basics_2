using Microsoft.EntityFrameworkCore;
using PokemonLib.Models;

namespace PokemonLib.Database;

public class PokemonDbContext: DbContext
{
    public PokemonDbContext(DbContextOptions<PokemonDbContext> options) : base(options)
    {
    }

    public DbSet<DbPokemon> Pokemons { get; set; }
    
    public DbSet<DbMove> Moves { get; set; }
}