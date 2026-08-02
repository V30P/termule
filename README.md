# Termule

A micro game engine for developing real-time terminal games.

![shooter demo](assets/shooter.gif "Shooter Demo")

## Overview
Termule is written in C# on .NET 10.0 with the goal of making terminal game development easy and enjoyable. With Termule, it is possible to build fluid and vibrant real-time games, rather than the turn-based or textual experiences typical of the terminal. Structurally, Termule can be classified as an object-oriented engine in the traditional sense, but with a few tricks to avoid the pitfalls of this model (see the [Architecture](#architecture) section for more details).

 As the sole developer of Termule, my main goal is to create something that is pleasant to both develop and use. As such, I strive as much as possible to keep the engine simple, low-dependency, and easily extensible. I have many plans for Termule and it's unlikely the project will ever be truly complete, so it's worth checking the [changelog](CHANGELOG.md) to see what's new.

## Contents
This repository contains the following projects:

1. [Engine](Engine)
   - Extensible software renderer
   - Performant terminal display system
   - Custom terminal input parser
   - Runtime resource loading
   - Fully documented API

2. [Tests](Tests)
   - Comprehensive xUnit test suite for the engine
   - Includes tests for core types and the majority of engine behavior

3. [Demos](Demos)
   - Five single-file, sample Termule programs
   - Easy-to-use CLI (to run locally, see the [Demos](#demos) section below)

## Architecture
Like many object-oriented game engines, Termule features game objects made up of components. This model is great because it favors composition over inheritance while still allowing for many of the usual object-oriented patterns. However, it has a few issues:

1. Components being responsible for all behavior and data quickly becomes messy.
2. There is no obvious place for global behavior that needs to tie into the game loop.

To alleviate these issues, Termule adds a complementary type to components: the `System`

### Systems
- Provide a home for global behavior and data
- Can only be installed, uninstalled, or swapped before the game runs
- Are limited to a single instance, easily accessed by other systems or components
- Allow complex behavior to be moved out of the component tree

### Components
- Provide a home for modular behavior and data
- Can be created, destroyed, or moved while a game is running
- Are grouped by game objects to enable collaboration
- Allow behavior to live close to data when it makes sense

An example game structure:

```
Game
 ├── Systems
 │    ├── RenderSystem
 │    ├── Terminal
 │    └── TerminalController
 └── World
      ├── Player
      │    ├── Transform
      │    ├── ContentRenderer
      │    ├── PlayerController
      │    └── Camera
      └── Enemy
           ├── Transform
           ├── ContentRenderer
           └── EnemyController
```

## Demos

Termule's `Demos` project provides a few sample programs which serve to demonstrate engine functionality and provide practical examples of using the API.

To run the demos yourself, first clone the full repository, then navigate to the [`Demos/`](Demos/) directory. The demo project can be run without installation via the .NET CLI:

```bash
# To learn how to use the Demos project
dotnet run -- --help

# To run a specific demo by name
dotnet run -- DEMO
```

For a demo's source code, look for the `.cs` file of the same name in [`Demos/Demos/`](Demos/Demos/).

## Getting Started
 The engine project itself can be found in the [`Engine/`](Engine/) directory. After adding it as a reference to your C# project, you can get started by constructing and running a basic game:

```csharp
using Termule.Engine.Core;

// Create a game instance
Game game = new();

// Install the default systems for your platform
game.Systems.InstallDefaults();

// Start the game
game.Run();
```

Before the game is run, systems and components can be added via `Game.Systems` and `Game.World` respectively:

```csharp
game.Systems.Install(new MySystem());
game.World.Add(new MyComponent());
```

For information about an element of the engine, hover over a Termule type or member to see its documentation.

## License
This project is licensed under the MIT License. See the LICENSE file for details.
