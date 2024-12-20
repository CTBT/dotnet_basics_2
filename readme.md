---

# .NET + C# Workshop
---
**Table of content:**

- [Basics](#item-1)
- [Working with the CLI](#item-2)
- [Dojo - Querying data (LINQ)](#item-3)
- [Testing (xunit)](#item-4)
- [Dojo - Writing test assertions](#item-5)
- [Implementing APIs](#item-6)

---
## Basics

- .net and c# versioning and releases
    - https://versionsof.net/
- sdk and runtime downloads
    - https://dotnet.microsoft.com/en-us/download
- recommended IDEs
    - Jetbrains Rider
    - VS Code + C# Dev Kit

--- 

## Working with the CLI

- doc: https://learn.microsoft.com/de-de/dotnet/core/tools/dotnet
- Usefull commands:
    - Create files and projects with ``dotnet new``
    - add a reference or package with ``dotnet add``
    - Build your code with ``dotnet build``
    - Run your app with ``dotnet run`` or ``dotnet watch``

--- 

## Taks 1: Project Setup

The goal is to setup a new console application that we can use for displaying data.

- Create a new solution 'PokemonSolution' (``dotnet new ...``)
- Create a new console project 'PokemonConsole' (``dotnet new ...``)
- add the project to the solution (``dotnet sln``)
- build
- run

## Taks 2:  Make an API Call and display the results

The goal ist to query the pokemon api and display the results in the console.

- Use the HttpClien class to make a get request to the pokemon api
- Deserialize the response to a list of pokemon
- Display the results in the console

### Task 3: Learn how to work with external nuget packages
The goal is to use the nuget package [Spectre.Console](https://www.nuget.org/packages/Spectre.Console)
to implement a pokemon manual.

- Add the nuget package to the project as a dependency
- Use the package to display the pokemon names in a selectable list
