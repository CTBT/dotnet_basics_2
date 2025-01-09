---

# dotnet + c# Workshop
---

- Basic Knowledge

- Working with the CLI

Coding:
- Create your first application
- Calling an external API
- Learn how to work with external packages
- Make your app interactive
- Make your Code reusable
- Provide your own API
- Add some features
- Make your app configurable for different environments
- Write a first test

---
## Basic Knowledge

- .net and c# versioning and releases
    - https://versionsof.net/
- sdk and runtime downloads
    - https://dotnet.microsoft.com/en-us/download
- recommended IDEs
    - Jetbrains Rider
    - VS Code + C# Dev Kit
- dotnet:
  - Languages: VB.net,C#,F#
  - Project Types: Libraries, Console Application, Web Application
  - Blazor
  - MAUI (Xamarin)

--- 

## Working with the CLI

- doc: https://learn.microsoft.com/de-de/dotnet/core/tools/dotnet
- Usefull commands:
    - Create files and projects with ``dotnet new``
    - add a reference or package with ``dotnet add``
    - Build your code with ``dotnet build``
    - Run your app with ``dotnet run`` or ``dotnet watch``

--- 

## Level 1: Create your first application

The goal is to setup a new console application that we can use for displaying data.

- Create a new solution 'PokemonSolution' (``dotnet new ...``)
- Create a new console project 'PokemonConsole' (``dotnet new ...``)
- add the project to the solution (``dotnet sln``)
- build
- run

---

### Level 1 completed - ⭐ 
Now we know how to setup a new dotnet project

--- 

## Level 2: Calling an external API

The goal is to implement a request to the pokemon api and display the results in the console.
The pokeapi pokemon list url can be found in the provided ``.http`` file.

- Use the HttpClient class to make a get request to the pokemon api to retrieve a list of pokemons
  ```c#
  var client = new HttpClient();
  ```
- Use the JsonSerializer class to map the request response to typed model
  ```c#
  JsonSerializer.Deserialize<PokemonList>(...)
  ```
- Display the results in the console (``Console.WriteLine``)

Hint: Be aware of cases sensitivity of models

---

### Level 2 completed - ⭐⭐ 
Now we know how to query external http endpoints and work with request results

---

## Level 3: Learn how to work with external packages
The goal ist to simplify your code by using an external nuget package that is not part of the .net sdk
Use the Refit library to make http calls to the pokemon api. Refit is a wrapper around the HttpClient class that ueses annotations to define external endpoints.

- Add the [Refit](https://www.nuget.org/packages/refit/) http client library to the project (with your IDE´s package explorer or ``dotnet add``).
- Annotate the `ÌPokemonApi` methods with a refit data annotation:
  ```c#
   [Get("/api/v2/pokemon?limit=10&offset=0")]
  ```
- Create a refit RestService and us it to replace your existing HttpClient request
  ```c#
  RestService.For<IPokemonApi>(host)
  ```
- Call the api again to request details of a random pokemon (width, height, moves)
- display those details in the console

---

### Level 3 completed -  ⭐⭐⭐ 
Now we know how to use external libraries to improve our code

---

## Level 4: Make your app interactive
Use the Spectre.Console nuget package to implement a pokedex in your console. 
Spectre.Console adds vizualizations and interactive componentes to your console.

- Add the [Spectre.Console](https://www.nuget.org/packages/Spectre.Console) console library to the project
- Use ``AnsiConsole.Write`` instead of ``Console.Write``
- Use the package to display the pokemon names in a selectable list ([docs](https://spectreconsole.net/))
``` c#
AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose a pokemon:")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more pokemon)[/]")
            .AddChoices(pokemonList.Results.Select(i=> i.Name)))
```
- display information about the selected pokemon
- bonus: styling

Hints:
- use a while loop to always provide the navigation menu to the user
- you can use a panel or a table to display the information properly

---

### Level 4 completed - ⭐⭐⭐⭐ 
Now we know how to create an interactive console application

---

## Level 5: Make your Code reusable

- Add a ``library`` project to your solution and move your code there
- Provide a ``PokemonService`` class with public methods ``GetPokemonList`` and ``GetPokemonDetails``

---

### Level 5 completed - ⭐⭐⭐⭐⭐ 
Now we know how to structure our code to make it reusable

---

## Level 6: Provide your own API

- Add a web api project to your solution and refrence your library
  ```c#
  <ProjectReference Include="..\PokemonLib\PokemonLib.csproj" />
  ```
- Declare mappings for the ``PokemonService`` methods to provide GET endpoints in your api
  ```c#
  app.MapGet("/pokemon", ...)
  ```

HINT:
- Use the dotnet dependency injection via constructor to provide a PokemonService instance
  ```c#
  [FromServices] PokemonService service
  ```

- Bonus: Keep your Program.cs clean

---

### Level 6 completed - ⭐⭐⭐⭐⭐⭐ 
We learned how to create an API project, define endpoints and use dependency injection

---

## Level 7: Add some features

 #### Provide an openapi UI for your team 
 - Reference the [Scalar](https://www.nuget.org/packages/Scalar.AspNetCore) package
 - Map the UI:
   
```c#
app.MapScalarApiReference();
```
 - add it to your launchsettings:
```
      "launchBrowser": true,
      "launchUrl": "http://localhost:5063/scalar/v1",
```
- test your endpoints with the UI

#### Make your api scalable with caching
- Add the output caching middleware ([Output Caching](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-9.0) )
  ```c#
  builder.Services.AddOutputCache();
  ...
  app.UseOutputCache();
  ```
- Activate caching for your endpoints
  ```c#
  .CacheOutput();
  ```

#### Refine your data types
- Change your endpoint result types so that the possible status codes are defined
```c#
Task<Results<NotFound, Ok<Pokemon>>>
```

```c#
return TypedResults.Ok(pokemon);
```

- Have a look into the openapi endpoint description to see the changes

#### Use the build-in logger of .net to write log messages
- Provid a new logger instance by using the generic logger interface and inject via constructor
```c#
ILogger<PokemonService> logger
```
- Add an ``information`` log whenever a pokemon will be requested from the external api and add his name to the log
- Additionally add a ``warning`` log if a pokemon was not found
- Configure your ``appsettings`` file to disable the information log level for the PokemonLib namespace:

---

### Level 7 completed - ⭐⭐⭐⭐⭐⭐⭐ 
We learned how to make our application more robust, scalabale and with well defined endpoints.

---

## Level 8: Make your app configurable for different environments
- Create an Interface IPokemonService and use it in the PokemonService
```c#
public class PokemonService : IPokemonService
```
- Create PokemonTestService that creates fake data
```c#
Enumerable.Range(1, 2000)
```
- Use the IPokemonTestService interface in your endpoints
- Add a configuration value that makes you choose the  type you want to use
```c#
var useTestData = builder.Configuration.GetValue<bool>("ServiceOptions:UseTestData");
builder.Services.AddScoped<IPokemonService, ...
```

---

### Level 8 completed - ⭐⭐⭐⭐⭐⭐⭐⭐ We learned how to use app configuration to configure our app for deployment environments

---

## Level 9 Write a first test
- Add a xunit test project for the PokemonLib and create a refrence to it
- Add a first test

---

### Level 9 completed - ⭐⭐⭐⭐⭐⭐⭐⭐⭐ 
We learned how write and run tests for our code

---

## Level 10 - Build your own UI with Blazor
- Add a Blazor Web App "PokemonPage" to your solution (RenderMode: Server, Interactivity Location: Global)
- Create a razor page that calls the IPokemonService.GetPokemonListAsync() method to display a list of pokemons and provide a search functionality
- Create a razor page that calls the IPokemonService.GetDetails() method to display pokemon detail information
- Bonus: Display the pokemon sprite by using it´s id: ``https://raw.githubusercontent.com/PokeAPI/sprites/master/sprites/pokemon/other/home/{id}``

Hints:
- Use the ``@inject`` directive to make the pokemon service available to your component by injecting the dependency
- You can always use the ``@`` to reference variables
- Event handlers can be implemented by using ``@onchange``, ``@oninput`` and other directives inside your html tags

---

### Level 10 completed - ⭐⭐⭐⭐⭐⭐⭐⭐⭐⭐ 
We learned how to create a web UI with Blazor

---

# Further things to know (to be continued)
- How to use different testing strategies
- How to observe our app with metrics and structured logs
- How to persist external data in your system
