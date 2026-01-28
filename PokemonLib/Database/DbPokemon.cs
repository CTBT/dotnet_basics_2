using System.ComponentModel.DataAnnotations;

namespace PokemonLib.Database;

public class DbPokemon
{
    public string? Name { get; set; }
    [Key]
    public int Id { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public IEnumerable<DbMove> Moves { get; set; }
}