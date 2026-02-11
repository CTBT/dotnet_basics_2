using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PokemonLib.Database;

public class DbPokemon
{
    [Key]
    public int Id { get; set; }
    public required string Name { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public IEnumerable<DbMove> Moves { get; set; }
}