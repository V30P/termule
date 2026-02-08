# Termule

A terminal-based game engine written in C#/.NET, focusing on extensible architecture and a pure OOP approach.

## Overview

Termule is written in C# on .NET 10.0. Its primary goal is to provide an easy-to-use, extensible API that supports straightforward future expansion. The engine has minimal dependencies, with nearly all systems implemented from scratch.

To achieve this extensibility, the core architecture separates runtime behavior into System and Component instances, which live within a configurable Game environment. All engine functionality is built on top of this expandable core architecture.

## Features

- Extensible, pure OOP core architecture  
- Sprite, line, and circle rendering capabilities  
- Performant terminal display using escape sequences  
- Keyboard and mouse input (mouse input not currently supported on Windows)  
- Resource loading system  
- Clean API with full XML documentation  

## Architecture

Termule splits runtime behavior into two major types: `System`s and `Component`s.

### Systems

- Live inside the `Game`'s `SystemManager`  
- Provide a home for global behavior and data  
- Can only be installed or swapped before the game runs  
- Easily accessible by other `System`s and `Component`s  

### Components

- Live inside the `Game`'s root `GameObject`  
- Can be created, destroyed, and moved during runtime  
- Exist within `GameObject`s, allowing for relative behavior  
- Enforce composition over inheritance  

This architecture allows for modular, yet digestible code and avoids too much behavior living inside components.

An example `Game` structure:

```text
Game
 ├── SystemManager
 │    ├── RenderSystem
 │    ├── Display
 │    └── Controller
 └── Root GameObject
      ├── Player
      │    ├── Transform
      │    └── ContentRenderer
      └── Enemy
           ├── Transform
           └── ContentRenderer
```

## Demos

For examples of what’s possible with Termule, see the demo project's README [here](./Demos/README.md).  

// Gif here

## Getting Started

After adding Termule as a reference in your C# project, you can get started by creating an instance of `Game`, installing the default `System`s, and then running it:

```csharp
static class Program
{
    static void Main()
    {
        IConfigurableGame game = Game.Create();
        game.Systems.UseDefaults();
        game.Run();
    }
}
```

Before the game is run, `System`s and `Component`s can be added to `Game.Systems` and `Game.Root`, respectively:

```csharp
game.Systems.Install(new MyTimerSystem());
game.Root.Add(new MyStopperComponent());
```

For information about individual elements of the engine, hover over a Termule type or member to see the documentation.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
