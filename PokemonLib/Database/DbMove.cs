using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PokemonLib.Database;

[Index(nameof(Name))]
public class DbMove
{
    [Key] 
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public DbPokemon Pokemon { get; set; }
}