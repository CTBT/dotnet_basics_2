using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonLib.Models;
using PokemonLib.Services;

namespace PokemonWebApi.Endpoints;

public static class PokemonDataEndpoints
{
    public static async Task<PokemonList> GetPokemonList([FromServices] IPokemonService service)
    {
        return await service.GetPokemonListAsync();
    }
    
    public static async Task<Results<NotFound, Ok<Pokemon>>> GetPokemon(
        [FromServices] IPokemonService service, 
        [FromRoute] string name)
    {
        var pokemon = await service.GetPokemonDetailsAsync(name);
        return pokemon is null ? TypedResults.NotFound() : TypedResults.Ok(pokemon);
    }
}