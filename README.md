# Termule

A micro game engine for creating responsive, real-time games in the terminal. 

## Overview

Termule is written in C# on .NET 10.0 and is built to bring a modern experience to terminal game development. Unlike many console-based libraries, Termule uses a traditional game loop rather than an event-driven model. This enables the creation of fast, fluid games rather than simple text-based experiences. Termule's API is designed with ease of use at the forefront and is built to be easily extensible, with a focus on clean, readable game code.

When I started work on Termule, I set out to explore the process of building a game engine and gain experience designing larger systems. An ongoing goal of mine is to implement the majority of engine functionality from scratch, minimizing external dependencies and keeping architectural decisions intentional. Over time, I hope to continue developing Termule with the goal of creating a complete game-making toolkit for the terminal.

## Contents

This repository contains the following projects:

1. [Engine](Engine)

   - Extensible render system
   - Performant terminal display via escape sequences
   - Cross-platform keyboard and mouse input
   - Runtime resource loading
   - Fully documented API

2. [Demos](Demos)

   - Five sample Termule programs
   - Single-file c# implementation for each demo
   - Easy-to-use CLI for configuration
   - Sample GIF collection

## Demos

For examples of what’s possible with Termule, see the demo project [here](Demos).

![shooter demo](Demos/gifs/shooter.gif)

More GIFs can be found in the demo collection's [README](Demos/README.md).

## Architecture

Termule splits runtime behavior into two major types: `System`s and `Component`s.

### Systems

- Live inside the game's `SystemManager`  
- Provide a home for global behavior and data  
- Can only be installed, uninstalled, or swapped before the game runs  
- Easily accessible by other systems or components 

### Components

- Live inside the game's root `GameObject`  
- Can be created, destroyed, and moved during runtime  
- Grouped by game objects to enable collaborative behavior
- Enforce composition over inheritance  

This architecture allows for modular, maintainable code that takes full advantage of C#'s syntax. Separating systems and components increases organization and stops behavior from living directly on game objects.

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
