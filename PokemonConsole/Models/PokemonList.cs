namespace PokemonConsole.Models;

public class PokemonList
{
    public int count { get; set; }
    public required List<PokemonListItem> results { get; set; }
}