// See https://aka.ms/new-console-template for more information
using PokemonConsole.Models;

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
    ArgumentNullException.ThrowIfNull(pokemonList);
}
catch (Exception)
{
    Console.WriteLine($"Pokemon list could not be fetched from the api (Url: {url}");
    throw;
}


// print the list of pokemon names:
pokemonList.Results.ForEach(pokemon => Console.WriteLine(pokemon.Name));