using System.Text.Json;
using PokemonConsole.Models;
using Spectre.Console;

AnsiConsole.MarkupLine("[underline red]Hello, this is your pokedex. Have fun![/]");

var pokeApiClient = new HttpClient
{
    BaseAddress = new Uri("https://pokeapi.co/api/v2/pokemon/")
};

PokemonList? pokemonList;
try
{
    // http call to get a list of pokemon from the pokemon api:
    var response = await pokeApiClient.GetStringAsync($"?limit=100&offset=0");
    var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    pokemonList = JsonSerializer.Deserialize<PokemonList>(response, jsonOptions);
    ArgumentNullException.ThrowIfNull(pokemonList);
}
catch (Exception)
{
    Console.WriteLine($"Pokemon list could not be fetched from the external api.(Url: {pokeApiClient.BaseAddress})");
    throw;
}

while (true)
{
    // print the list of pokemon names:
    var pokemonName = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose a pokemon:")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more pokemon)[/]")
            .AddChoices(pokemonList.Results.Select(i=> i.Name)));

    Pokemon? selectedPokemon;
    try
    {
        var response = await pokeApiClient.GetStringAsync($"{pokemonName}");
        var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        selectedPokemon = JsonSerializer.Deserialize<Pokemon>(response, jsonOptions);
        ArgumentNullException.ThrowIfNull(selectedPokemon);
    }
    catch (Exception)
    {
        Console.WriteLine($"Pokemon could not be fetched from the api (Url: {pokeApiClient.BaseAddress})");
        throw;
    }


// Echo the fruit back to the terminal
    AnsiConsole.MarkupLine($"[green]I agree. I also like {pokemonName}! It can perform the following moves:[/]");

// Load image from url and display it in the console:
    var imageClient = new HttpClient();
    var stream = await imageClient.GetStreamAsync(selectedPokemon.Sprites.Other.Showdown.Front_Default);
    
    

    var image = new CanvasImage(stream);
    image.MaxWidth = 30;
    AnsiConsole.Write(new Rule($"[red]{pokemonName}[/]"));
    AnsiConsole.Write(image);
    AnsiConsole.Write(new Rule());
    

}
