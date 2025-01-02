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
    Console.WriteLine($"Height of {details.Name}: {details.Height}");
    Console.WriteLine($"Weight of {details.Name}: {details.Weight}");
    Console.WriteLine($"Number of Moves of {details.Name}: {details.Moves.Count()}");
    Console.WriteLine($"Number of Moves of {details.Name}: {string.Join(",",details.Moves.Select(i => i.Move.Name))}");
}
catch (ApiException e)
{
    Console.WriteLine(e.StatusCode == HttpStatusCode.NotFound
        ? $"The requested resource could not be found: {e.Uri} "
        : $"Pokemon api call to {e.Uri} failed");
    throw;
}

