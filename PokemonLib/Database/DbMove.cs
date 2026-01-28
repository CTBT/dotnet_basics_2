using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PokemonLib.Database;

public class DbMove
{
    [Key] 
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DbPokemon Pokemon { get; set; }
}