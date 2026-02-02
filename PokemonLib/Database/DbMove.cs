using System.ComponentModel.DataAnnotations;

namespace PokemonLib.Database;

public class DbMove
{
    [Key] 
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DbPokemon Pokemon { get; set; }
}