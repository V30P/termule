# Termule Demos

This directory contains the demo project for the Termule game engine. Each demo highlights a specific engine capability
and serves as a simple but practical example of interacting with the Termule API. For this reason, demo source code
prioritizes clarity and elegance over raw performance.

For information about the engine itself, see the main README [here](../README.md).

## Demos

Below is a list of the included demos and the functionality they demonstrate.

- *gradient* - full RGB colors

![gradient demo](gifs/gradient.gif)

- *lightning* - line rendering

![lightning demo](gifs/lightning.gif)

- *raindrops* - circle rendering

![raindrops demo](gifs/raindrops.gif)

- *screensaver* - runtime loading and positioning of content

![screensaver demo](gifs/screensaver.gif)

- *shooter* - input handling

![shooter demo](gifs/shooter.gif)

## Running

After cloning the full Termule repository, run the following command from the repository root to learn how to use this
demo project:

```bash
dotnet run --project Demos --help
```

Or to run a specific demo by name:

```bash
dotnet run --project Demos <demo>
```