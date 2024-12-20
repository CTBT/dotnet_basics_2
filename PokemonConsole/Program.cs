// See https://aka.ms/new-console-template for more information

using System.Text.Json;

Console.WriteLine(" ----- Hello, this is the pokemon manual -----");

// http call to get a list of pokemon from the pokemon api:
var client = new HttpClient();
const string url = "https://pokeapi.co/api/v2/pokemon?limit=2000&offset=0";
var response = await client.GetStringAsync(url);

var jsonOptions = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

PokemonList? pokemonList;
try
{
    pokemonList = JsonSerializer.Deserialize<PokemonList>(response, jsonOptions);
}
catch (JsonException e)
{
    Console.WriteLine($"Pokemon list could not be fetched from the api (Url: {url}");
    throw;
}



foreach (var pokemon in pokemonList?.Results)
{
    Console.WriteLine(pokemon.Name);
}

public class PokemonList
{
    public required List<Pokemon> Results { get; set; }
}

public class Pokemon
{
    public required string Name { get; set; }
}


