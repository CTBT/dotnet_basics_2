using Microsoft.Extensions.DependencyInjection;
using PokemonLib.Database;
using PokemonLib.PokeApi.Services;

namespace PokemonLib;

using Microsoft.Extensions.Hosting;
    
public class PokemonSyncJob: IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IPokemonApi _pokemonApi;

    public PokemonSyncJob(IServiceProvider serviceProvider, IPokemonApi pokemonApi)
    {
        _serviceProvider = serviceProvider;
        _pokemonApi = pokemonApi;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<PokemonDbContext>();
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.EnsureCreatedAsync(cancellationToken);
        var pokemonList = await _pokemonApi.GetPokemonListAsync(100, 0);

        var pokemonNames = pokemonList.Results.Select(i => i.Name).ToList();
        var pokemons = new Dictionary<string, DbPokemon>();

        foreach (var pokemonName in pokemonNames)
        {
            var pokemon = await _pokemonApi.GetPokemonDetailsAsync(pokemonName);
            if (pokemon.Content is not null)
            {
                pokemons.Add(pokemon.Content.Name, new DbPokemon()
                {
                    Name = pokemon.Content.Name,
                    Height = pokemon.Content.Height,
                    Weight = pokemon.Content.Weight,
                    Moves = pokemon.Content.Moves
                        .Select(m => new DbMove() { Name = m.Move.Name })
                        .ToList()
                });
            }
        }
        
        context.Pokemons.AddRange(pokemons.Values);
       
        await context.SaveChangesAsync(cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
