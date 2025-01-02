using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PokemonLib.Models;
using PokemonLib.Services;
using Refit;

namespace PokemonWebApi.Endpoints;

public static class PokemonDataEndpoints
{
    public static async Task<PokemonList> GetPokemonList([FromServices] PokemonService service)
    {
        return await service.GetPokemonListAsync();
    }
    
    public static async Task<Results<NotFound, Ok<Pokemon>>> GetPokemon([FromServices] PokemonService service, [FromRoute] string name)
    {
        Pokemon? pokemon = null;
        try
        {
            pokemon = await service.GetPokemonDetails(name);
        }
        catch (ApiException e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                return TypedResults.NotFound();
            }
        }

        return TypedResults.Ok(pokemon);
    }
}