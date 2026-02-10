# Termule

A lightweight game engine written in C#/.NET, built for responsive, real-time games in the terminal.

## Overview

Termule is written in C# on .NET 10.0. It is designed similarly to a traditional game engine, with a primary game loop rather than the event driven approach of other console-based engines. This enables the creation of fast, fluid games for terminal environments.

The API is designed to be easy-to-use and extensible, with a focus on clean user-side code. The project also relies on minimal dependencies, with nearly all systems implemented from scratch. Most of this from-scratch functionality is built on top of Termule's core System and Component types which neatly organize runtime behavior and data.

## Features

- Clean, flexible core architecture  
- Sprite, line, and circle rendering capabilities  
- Performant terminal display using escape sequences  
- Keyboard and mouse input (mouse input not currently supported on Windows)  
- Resource loading system  
- API fully documented with XML comments

## Architecture

Termule splits runtime behavior into two major types: `System`s and `Component`s.

### Systems

- Live inside the `Game`'s `SystemManager`  
- Provide a home for global behavior and data  
- Can only be installed, uninstalled, or swapped before the game runs  
- Easily accessible by other `System`s or `Component`s  

### Components

- Live inside the `Game`'s root `GameObject`  
- Can be created, destroyed, and moved during runtime  
- Grouped by `GameObject`s to enable collaborative behavior
- Enforce composition over inheritance  

This architecture allows for modular, yet digestible code and avoids the classic problem of too much behavior living in components.

An example `Game` structure:

```
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

For examples of what’s possible with Termule, see the demo project [here](Demos).  

![shooter demo](Demos/gifs/shooter.gif)

More GIFs can be found in the demo project's [README](Demos/README.md).

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

For information about individual elements of the engine, hover over a Termule type or member to see its documentation.

## License

This project is licensed under the MIT License. See the LICENSE file for details.
