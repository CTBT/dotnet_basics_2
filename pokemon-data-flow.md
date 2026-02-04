# Pokemon Datenfluss Diagramm

## Übersicht
Dieses Diagramm beschreibt den Datenfluss, wenn Pokemon-Daten von der PokemonPage abgefragt werden.

## Architektur-Diagramm

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                              BLAZOR SERVER SIDE                             │
│                                                                             │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │                       PokemonPage (Blazor)                            │  │
│  │                  Components/Pages/Pokemon.razor                       │  │
│  │                                                                       │  │
│  │  • Empfängt Name als Route-Parameter (/pokemon/{name})              │  │
│  │  • Zeigt Pokemon-Details an                                          │  │
│  │  • Rendert UI mit @StreamRendering                                   │  │
│  └───────────────────┬───────────────────────────────────────────────────┘  │
│                      │                                                       │
│                      │ @inject IPokemonService                              │
│                      │                                                       │
│                      ▼                                                       │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │                        IPokemonService                                │  │
│  │              (Interface - Dependency Injection)                       │  │
│  │                                                                       │  │
│  │  Methoden:                                                            │  │
│  │  • GetPokemonListAsync() → PokemonList                               │  │
│  │  • GetPokemonDetailsAsync(name) → Pokemon?                           │  │
│  └───────────────────┬───────────────────────────────────────────────────┘  │
│                      │                                                       │
│                      │ Implementation (DI: Scoped)                          │
│                      │                                                       │
│                      ▼                                                       │
│  ┌───────────────────────────────────────────────────────────────────────┐  │
│  │                    PokemonDbCacheService                              │  │
│  │         PokemonLib/PokeApi/Services/PokemonDbCacheService.cs         │  │
│  │                                                                       │  │
│  │  Cache-Strategie:                                                     │  │
│  │  1. Prüfe SQLite Datenbank                                           │  │
│  │  2. Falls nicht vorhanden → API Call                                 │  │
│  │  3. Speichere API-Ergebnis in DB                                     │  │
│  │  4. Gebe Daten zurück                                                │  │
│  └───────┬───────────────────────────────────────────┬───────────────────┘  │
│          │                                           │                       │
│          │ DbContext (EF Core)                       │ IPokemonApi (Refit)  │
│          │                                           │                       │
│          ▼                                           ▼                       │
│  ┌──────────────────────────┐      ┌─────────────────────────────────────┐  │
│  │   PokemonDbContext       │      │         IPokemonApi                 │  │
│  │  (Entity Framework Core) │      │  (Refit HTTP Client Interface)      │  │
│  │                          │      │                                     │  │
│  │  DbSet<DbPokemon>        │      │  GET /api/v2/pokemon?limit&offset   │  │
│  │  DbSet<DbMove>           │      │  GET /api/v2/pokemon/{name}         │  │
│  └──────────┬───────────────┘      └──────────────┬──────────────────────┘  │
│             │                                     │                          │
└─────────────┼─────────────────────────────────────┼──────────────────────────┘
              │                                     │
              │                                     │
              ▼                                     ▼
    ┌──────────────────┐              ┌──────────────────────────┐
    │  SQLite Database │              │  Externe PokeAPI         │
    │   pokemon.db     │              │  https://pokeapi.co      │
    │                  │              │                          │
    │  Tabellen:       │              │  REST API Endpoints      │
    │  • Pokemons      │              │  • Pokemon Liste         │
    │  • Moves         │              │  • Pokemon Details       │
    └──────────────────┘              └──────────────────────────┘
```

## Detaillierter Datenfluss

### Szenario 1: Pokemon-Details (Cache Miss)

```
1. Benutzer navigiert zu /pokemon/pikachu
   ↓
2. Pokemon.razor lädt
   ↓
3. OnInitializedAsync() wird aufgerufen
   ↓
4. PokemonService.GetPokemonDetailsAsync("pikachu")
   ↓
5. PokemonDbCacheService prüft SQLite DB
   • Query: SELECT * FROM Pokemons WHERE Name = 'pikachu'
   ↓
6. Cache Miss → Kein Eintrag gefunden
   ↓
7. HTTP Request via Refit IPokemonApi
   • GET https://pokeapi.co/api/v2/pokemon/pikachu
   ↓
8. API Response empfangen
   ↓
9. Daten in DB speichern
   • DbPokemon Objekt erstellen
   • DbMove Objekte für alle Moves erstellen
   • context.Pokemons.Add(dbPokemon)
   • context.SaveChangesAsync()
   ↓
10. Pokemon Objekt zurückgeben
    ↓
11. Blazor rendert die Komponente mit den Daten
```

### Szenario 2: Pokemon-Details (Cache Hit)

```
1. Benutzer navigiert zu /pokemon/pikachu
   ↓
2. Pokemon.razor lädt
   ↓
3. OnInitializedAsync() wird aufgerufen
   ↓
4. PokemonService.GetPokemonDetailsAsync("pikachu")
   ↓
5. PokemonDbCacheService prüft SQLite DB
   • Query: SELECT * FROM Pokemons INCLUDE Moves WHERE Name = 'pikachu'
   ↓
6. Cache Hit → Eintrag gefunden!
   ↓
7. DbPokemon → Pokemon Mapping
   • Konvertiere DbPokemon zu Pokemon Model
   • Konvertiere DbMove[] zu MoveListItem[]
   ↓
8. Pokemon Objekt direkt zurückgeben (KEIN API Call!)
   ↓
9. Blazor rendert die Komponente mit den Daten
```

### Szenario 3: Pokemon-Liste

```
1. OnInitializedAsync() ruft GetPokemonListAsync() auf
   ↓
2. PokemonDbCacheService.GetPokemonListAsync()
   ↓
3. Direkter API Call (KEINE Cache-Logik für Liste)
   • GET https://pokeapi.co/api/v2/pokemon?limit=10000&offset=0
   ↓
4. PokemonList zurückgeben
   ↓
5. Dropdown wird mit allen Pokemon Namen gefüllt
```

## Technologie-Stack

| Komponente | Technologie | Zweck |
|------------|------------|--------|
| **Frontend** | Blazor Server (.NET 8+) | Server-Side Rendering |
| **Rendering Mode** | Interactive Server + StreamRendering | Streaming von Daten während Laden |
| **DI Container** | ASP.NET Core DI | Service Registrierung |
| **HTTP Client** | Refit | Typisierter HTTP Client für PokeAPI |
| **ORM** | Entity Framework Core | Datenbankzugriff |
| **Database** | SQLite | Lokaler Cache für Pokemon-Daten |
| **UI Components** | Microsoft FluentUI | UI-Komponenten (FluentSelect) |

## Service Registrierung (Program.cs)

```csharp
// Blazor Server Components
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// HTTP Client für externe API (Refit)
builder.Services.AddRefitClient<IPokemonApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://pokeapi.co"));

// Service mit Cache-Logik (Scoped = pro Request)
builder.Services.AddScoped<IPokemonService, PokemonDbCacheService>();

// SQLite Datenbank Context
builder.Services.AddDbContext<PokemonDbContext>(options =>
    options.UseSqlite("Data Source=pokemon.db"));
```

## Datenmodelle

### Blazor/API Models (PokemonLib.Models)
- `Pokemon` - Vollständige Pokemon-Details von API
- `PokemonList` - Liste aller Pokemon
- `PokemonListItem` - Einzelnes Pokemon in der Liste
- `MoveListItem` - Pokemon-Move
- `Move` - Move-Details

### Datenbank Models (PokemonLib.Database)
- `DbPokemon` - Pokemon Entity für EF Core
- `DbMove` - Move Entity für EF Core

## Vorteile dieser Architektur

✅ **Performance**: Nur ein API-Call pro Pokemon (danach aus DB)  
✅ **Offline-Fähigkeit**: Gecachte Daten verfügbar ohne Internet  
✅ **Skalierbarkeit**: Reduziert Last auf externe API  
✅ **Separation of Concerns**: Klare Trennung zwischen Layers  
✅ **Testbarkeit**: Interfaces ermöglichen Mocking  
✅ **Dependency Injection**: Lose Kopplung zwischen Komponenten  

## Server-Side Rendering Details

**Blazor Server** mit **StreamRendering**:
- Server rendert HTML und sendet es zum Browser
- SignalR WebSocket-Verbindung für UI-Updates
- `@attribute [StreamRendering]` ermöglicht progressives Rendering
- "Loading..." wird initial gezeigt, bis Daten geladen sind
- `StateHasChanged()` triggert UI-Update über SignalR

**Rendering-Flow**:
```
Browser Request → ASP.NET Core Server → Blazor Component
                                      ↓
                               Async Data Load
                                      ↓
                               SignalR Update → Browser erhält HTML-Diff
```
