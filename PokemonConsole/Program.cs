using System.Net;
using PokemonConsole.Services;
using Refit;

Console.WriteLine(" ----- Hello, this is the pokemon manual -----");

// http call to get a list of pokemon from the pokemon api:

var host = "https://pokeapi.co";
var pokemonService = RestService.For<IPokemonApi>(host);
try
{
    var pokemonList = await pokemonService.GetPokemonListAsync();
    
    // print the list of pokemon names:
    pokemonList.Results.ForEach(pokemon => Console.WriteLine(pokemon.Name));
    
    Console.WriteLine("------");
    
    // get details of a random pokemon from the list:
    var randomId = new Random().Next(0, pokemonList.Results.Count - 1);
    var details = await pokemonService.GetPokemonDetails(pokemonList.Results[randomId].Name);
    Console.WriteLine($"Details of {details.Name}:");
    Console.WriteLine($"Height: {details.Height}");
    Console.WriteLine($"Weight: {details.Weight}");
    Console.WriteLine($"Number of Moves: {details.Moves.Count()}");
    Console.WriteLine($"Moves of {details.Name}: {string.Join(",",details.Moves.Take(3).Select(i => i.Move.Name))}");
}
catch (ApiException e)
{
    Console.WriteLine(e.StatusCode == HttpStatusCode.NotFound
        ? $"The requested resource could not be found: {e.Uri} "
        : $"Pokemon api call to {e.Uri} failed");
    throw;
}

