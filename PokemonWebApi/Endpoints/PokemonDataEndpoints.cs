using Microsoft.AspNetCore.Mvc;
using PokemonLib.Models;
using PokemonLib.Services;

namespace PokemonWebApi.Endpoints;

public static class PokemonDataEndpoints
{
    public static async Task<PokemonList> GetPokemonList([FromServices] PokemonService service)
    {
        return await service.GetPokemonListAsync();
    }
    
    public static async Task<Pokemon> GetPokemon([FromServices] PokemonService service, [FromRoute] string name)
    {
        return await service.GetPokemonDetailsAsync(name);
    }
}