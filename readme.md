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


⭐ Now we know what dotnet is and for what it is used

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

Level 1: ⭐⭐ Now we know how to setup a new dotnet project
--- 

## Task 2:  Make an API Call and display the results

The goal ist to query the pokemon api and display the results in the console.

- Use the HttpClient class to make a get request to the pokemon api
- Deserialize the response to a list of pokemon (use typed models)
- Display the results in the console


Level 2: ⭐⭐ Now we know how to query apis an work with data types
---

## Task 3: Learn how to work with external libraries
The goal ist to use external nuget packages to add functionallity to your project that is not included in the .net sdk
Use the Refit library to make http call to the pokemon api. Refit is a wrapper around the HttpClient class that ueses annotations to define external endpoints.

- Add the [Refit](https://www.nuget.org/packages/refit/) http client library to the project
- Use the package to replace the HttClient call in your code
- Call the api again to request details of a random pokemon (width, height, moves)
- display those details in the console

Level 3: ⭐⭐⭐ Now we know how to be even more productive with dotnet by using external libraries

---

## Task 4: Implement the pokedex
Use the Spectre.Console nuget package to implement a pokedex in your console. 
Spectre.Console adds vizualizations and interactive componentes to your console.

- Add the [Spectre.Console](https://www.nuget.org/packages/Spectre.Console) console library to the project
- Use the package to display the pokemon names in a selectable list ([docs](https://spectreconsole.net/))
- Make another the Display pokemon detail information, for example 'stats'


Level 4: ⭐⭐⭐⭐ Now we know how to be even more productive with dotnet by using external libraries

