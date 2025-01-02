// See https://aka.ms/new-console-template for more information
using PokemonConsole.Models;
using PokemonConsole.Services;
using Refit;

Console.WriteLine(" ----- Hello, this is the pokemon manual -----");

// http call to get a list of pokemon from the pokemon api:

var host = "https://pokeapi.co";
var pokemonService = RestService.For<IPokemonApi>(host);
PokemonList? pokemonList;
try
{
    pokemonList = await pokemonService.GetPokemonListAsync();
}
catch (ApiException e)
{
    Console.WriteLine($"Pokemon list could not be fetched from {e.Uri}");
    throw;
}

// print the list of pokemon names:
pokemonList.Results.ForEach(pokemon => Console.WriteLine(pokemon.Name));