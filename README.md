# Termule

A micro game engine for creating responsive, real-time games in the terminal. 

## Overview

Termule is written in C# on .NET 10.0 and is built from the ground up to bring a modern experience to terminal game development. The engine is primarily designed for the creation of fluid and vibrant realtime games, as opposed to the static, text-based experiences typical of the terminal. For an overview of the engine's functionality, see the ["Contents"](#contents) section below or take a look at the [changelog](CHANGELOG.md) to see what's new.

As the sole developer of Termule, my main goal has always been to create something that is pleasant to both develop and work with. As such, I strive as much as possible to keep the engine low-dependency and easily extensible. Termule can be broadly classified as a component-based engine, but with an additional system type that helps alleviate some of the pains of this model (see the ["Architecture"](#architecture) section for more details).

## Contents

This repository contains the following projects:

1. [Engine](Engine)

   - Extensible render system
   - Escape-sequence-based terminal display
   - Cross-platform keyboard and mouse input
   - Runtime resource loading
   - Fully documented API

2. [Tests](Tests)

    - Comprehensive test suite for the main engine 
    - Built on xUnit.net for broad compatibility
    - Includes tests for all core types and the majority of other behavior
   
3. [Demos](Demos)

   - Five sample Termule programs
   - Single-file implementation for each demo
   - Easy-to-use CLI

## Demos

This repository includes a collection of self contained, single-file demo systems bundled into a single CLI. These demos serve both to demonstrate engine functionality and provide examples of using the API. For a demo's source code, look for the `.cs` file of the same name in [`Demos/Demos`](Demos/Demos/).

![shooter demo](assets/shooter.gif "Shooter Demo")

### Running the Demo Project

After cloning the full repository, navigate to the [`Demos`](Demos) directory. The demo project can be run without installation
via the .NET CLI:

```bash
# To learn more about the Demos CLI 
dotnet run --help

# To run a specific demo by name
dotnet run --project DEMO
```

## Architecture

In keeping with the model of a component-based game engine, Termule has typical implementations of components and game objects. This architecture is great because it favors composition over inheritance while still allowing the usual object-oriented patterns. However, it has a few issues:

1. There is no obvious place for global behavior that needs to tie into the game loop.
2. Having all behavior live on components results in a confusing, spaghetti-like mess at scale.

To solve this, Termle splits runtime behavior into two major types: `System`s and `Component`s.

### Systems

- Live inside the game's `SystemManager`  
- Provide a home for global behavior and data  
- Can only be installed, uninstalled, or swapped before the game runs  
- Consist of a single instance easily accessed by other systems or components 
- Allow complex behavior to be moved out of the component tree

### Components

- Live inside the game's root `GameObject`  
- Provide a home for modular behavior and data
- Can be created, destroyed, and moved during runtime  
- Grouped by game objects to enable collaboration
- Allow behavior to live close to data *when it makes sense*

An example `Game` structure:

```
Game
 ├── SystemManager
 │    ├── RenderSystem
 │    ├── Display
 │    └── Keyboard
 └── Root GameObject
      ├── Player
      │    ├── Transform
      │    ├── Camera
      │    └── ContentRenderer
      └── Enemy
           ├── Transform
           └── ContentRenderer
```

## Getting Started

After adding Termule as a reference in your C# project, you can get started by constructing and running a basic game:

```csharp
using Termule.Engine.Core;

// Create a game instance
var game = Game.Create();

// Install the default systems for your platform
game.Systems.UseDefaults();

// Start the game
game.Run();
```

Before the game is run, systems and components can be added to `Game.Systems` and `Game.Root` respectively:

```csharp
game.Systems.Install(new MySystem());
game.Root.Add(new MyComponent());
```

For information about an individual element of the engine, hover over a Termule type or member to see its documentation.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
