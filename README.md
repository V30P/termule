# Termule

An SDK for developing real-time terminal games.

![shooter demo](assets/shooter.gif "Shooter Demo")

## Overview
The Termule SDK is written in C# on .NET 10.0 with the goal of making terminal game development easy and enjoyable. With Termule, it is possible to build fluid and vibrant real-time games, rather than the turn-based or textual experiences typical of the terminal. As the sole developer of Termule, my goal is to create something that is pleasant to both develop and use. As such, I strive as much as possible to keep the SDK simple, low-dependency, and accessible. I have many plans for Termule and it's unlikely the project will ever be truly complete, so it's worth checking the [changelog](CHANGELOG.md) to see what's new.

## Contents
This repository contains the following projects:

1. [Engine](Engine) | [README](Engine/README.md)
   - Extensible software renderer
   - Performant terminal display system
   - Custom terminal input parser
   - Runtime resource loading
   - Fully documented API

2. [Demos](Demos) | [README](Demos/README.md)
   - Five single-file, sample programs built with Termule
   - Easy-to-use CLI, packaged as a .NET tool
   - Compiles to a single, self-contained file

3. [Tests](Tests)
   - Comprehensive xUnit test suite for the engine
   - Includes 300+ tests

## Quickstart
The termule engine is published on NuGet as `Termule.Engine`. To add it to your C# project use:

```bash
dotnet package add Termule.Engine
```

Now that your project has access to the engine library, you can get started by constructing and running a basic game.

```csharp
using Termule.Engine.Core;

// Create a game instance
Game game = new();

// Install the default systems for your platform
game.Systems.InstallDefaults();

// Start the game
game.Run();
```

Before the game is run, systems and components can be added via `Game.Systems` and `Game.World` respectively.

```csharp
game.Systems.Install(new MySystem());
game.World.Add(new MyComponent());
```

For information about an element of the engine, hover over a Termule type or member to see its documentation.

## Contributing
This repository is open to contributions. See the the contributing guidelines [here](CONTRIBUTING.md).

## License
This project is licensed under the MIT License. See the [LICENSE file](LICENSE) for details.
