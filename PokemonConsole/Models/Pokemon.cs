using System.Runtime.CompilerServices;

namespace PokemonConsole.Models;

public class Pokemon
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public IEnumerable<MoveListItem> Moves { get; set; }
}