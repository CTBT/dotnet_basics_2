namespace PokemonLib.Models;

public class Pokemon
{
    public string? Name { get; set; }
    public int Id { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public required IEnumerable<MoveListItem> Moves { get; set; }
}