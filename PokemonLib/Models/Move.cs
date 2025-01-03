using System.Diagnostics.CodeAnalysis;

namespace PokemonLib.Models;

public class Move
{
    public Move()
    {
    }

    [SetsRequiredMembers]
    public Move(string name)
    {
        Name = name;
    }

    public required string Name { get; set; }
}