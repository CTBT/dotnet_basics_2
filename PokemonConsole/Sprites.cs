namespace PokemonConsole.Models;

public class Sprites
{
    public required string Front_Default { get; set; }
    public Other Other { get; set; }
}


public class Other
{
    public Showdown Showdown { get; set; }
}

public class Showdown
{
    public string Front_Default { get; set; }
}