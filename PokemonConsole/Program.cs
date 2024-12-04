using System.Text.Json;
using PokemonConsole.Models;

Console.WriteLine("Hello, World!");

PokemonList? pokemonList = JsonSerializer
    .Deserialize<PokemonList>(File.ReadAllText("testdata/pokemon.json"));
var pokemonNames = pokemonList.results
    .Select(i => i.name)
    .ToList();

    Console.WriteLine(string.Join(",", pokemonNames));