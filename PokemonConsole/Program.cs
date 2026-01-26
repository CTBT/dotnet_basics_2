using System.Net;
using PokemonLib.Services;
using Refit;
using Spectre.Console;

AnsiConsole.MarkupLine("[underline green]Hello, this is your pokedex. Have fun![/]");

// http call to get a list of pokemon from the pokemon api:
var pokemonApi = RestService.For<IPokemonApi>("https://pokeapi.co");
var pokemonService = new PokemonService(pokemonApi);
try
{
    var pokemonList = await pokemonService.GetPokemonListAsync();
    while (true)
    {
        // print the list of pokemon names:
        var pokemonName = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("Choose a pokemon:")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more pokemon)[/]")
                .AddChoices(pokemonList.Results.Select(i=> i.Name)));
    
        AnsiConsole.WriteLine($"You selected: {pokemonName}");
        // get details of a random pokemon from the list:
        var details = await pokemonService.GetPokemonDetailsAsync(pokemonName);
        // Create a table with pokemon attributes
        var table = new Table()
            .RoundedBorder()
            .BorderColor(Color.Blue);
        table.AddColumn("Attribute");
        table.AddColumn("Value");
        table.AddRow("Height", details.Height.ToString());
        table.AddRow("Weight", details.Weight.ToString());
        table.AddRow("Number of Moves", details.Moves.Count().ToString());
        table.AddRow("Example moves", string.Join(",",details.Moves.Take(3).Select(i => i.Move.Name)));
        AnsiConsole.Write(table);
    }
}
catch (ApiException e)
{
    AnsiConsole.MarkupLine(e.StatusCode == HttpStatusCode.NotFound
        ? $"[red]The requested resource could not be found: {e.Uri}[/]"
        : $"[red]Pokemon api call to {e.Uri} failed[/]");
    throw;
}

