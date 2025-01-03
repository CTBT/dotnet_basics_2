---

# dotnet + c# Workshop
---
**Table of content:**

- [Basics](#item-1)
- [Working with the CLI](#item-2)
- [Task 1: Project Setup](#item-3)
- [Task 2:  Make an API Call and display the results](#item-4)
- [Task 3: Learn how to work with external libraries](#item-5)
- [Task 4: Implement the pokedex](#item-6)

---
## Basics

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

## Task 1: Project Setup

The goal is to setup a new console application that we can use for displaying data.

- Create a new solution 'PokemonSolution' (``dotnet new ...``)
- Create a new console project 'PokemonConsole' (``dotnet new ...``)
- add the project to the solution (``dotnet sln``)
- build
- run

Level 1 - ⭐ Now we know how to setup a new dotnet project

--- 

## Task 2:  Make an API Call and display the results

The goal ist to query the pokemon api and display the results in the console.

- Use the HttpClient class to make a get request to the pokemon api
- Deserialize the response to a list of pokemon (use typed models)
- Display the results in the console


Level 2 - ⭐⭐ Now we know how to query apis an work with data types

---

## Task 3: Learn how to work with external libraries
The goal ist to use external nuget packages to add functionallity to your project that is not included in the .net sdk
Use the Refit library to make http call to the pokemon api. Refit is a wrapper around the HttpClient class that ueses annotations to define external endpoints.

- Add the [Refit](https://www.nuget.org/packages/refit/) http client library to the project
- Use the package to replace the HttClient call in your code
- Call the api again to request details of a random pokemon (width, height, moves)
- display those details in the console

```c#
var pokemonService = RestService.For<IPokemonApi>(host);
var pokemonList = await pokemonService.GetPokemonListAsync();
```

Level 3 -  ⭐⭐⭐ Now we know how to be even more productive with dotnet by using external libraries

---

## Task 4: Implement the pokedex
Use the Spectre.Console nuget package to implement a pokedex in your console. 
Spectre.Console adds vizualizations and interactive componentes to your console.

- Add the [Spectre.Console](https://www.nuget.org/packages/Spectre.Console) console library to the project
- Use the package to display the pokemon names in a selectable list ([docs](https://spectreconsole.net/))
- display information about the choosen pokemon
- navigate the user back to the prompt after showing the pokemon details
- bonus: styling

``` c#
AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title("Choose a pokemon:")
            .PageSize(10)
            .MoreChoicesText("[grey](Move up and down to reveal more pokemon)[/]")
            .AddChoices(pokemonList.Results.Select(i=> i.Name)))
```

Level 4 - ⭐⭐⭐⭐ Now we know how to be even more productive with dotnet by using external libraries

---

## Task 5: Make your Code reusable

- Add a library project to your solution and move your code there
- Provide a PokemonService class with public methods

Level 5 - ⭐⭐⭐⭐⭐ Now we know how to structure our code to make it reusable

---

## Task 6: Provide your own API

- Add a web api project to your solution and refrence your library
- Implement endpoints for your public PokemonService methods
  ! Use the dotnet dependency injection to provide a PokemonService instance

## Task 7: Focus on quality

#### Make your api scalable with [Output Caching](https://learn.microsoft.com/en-us/aspnet/core/performance/caching/output?view=aspnetcore-9.0) 
- Add Caching middleware
  ```c#
  app.UseOutputCache();
  ```
- Activate caching for your endpoints
  ```c#
  .CacheOutput();
  ```

#### Refine your data types
- Define result types:
```c#
Task<Results<NotFound, Ok<Pokemon>>>
```

```c#
return TypedResults.Ok(pokemon);
```

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

#### Configure Logging
- Use an instance of ILogger to write log messages
```c#
ILogger<PokemonService>
```

- Configure your log levels:
```c#
  {
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "PokemonWebApi": "Warning"
    }
  }
}
```

Level 6 - ⭐⭐⭐⭐⭐⭐ We learned how to make our application more robust, scalabale and with well defined endpoints.

## Task 7: Make it testable
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

Level 7 - ⭐⭐⭐⭐⭐⭐ Our application can now be tested without production data and has testable code.

Further things to know (to be continued)
- How to write unit and integration tests
- How to export metrics, structured logs to external systems an how to work with them
- How to persist external data in your system

