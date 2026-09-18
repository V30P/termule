using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Types;

Game game = new();
game.Systems.InstallDefaults();

game.World.Add(
    new Camera(),
    new Transform(),
    new ContentRenderer<Text>()
    {
        Content = new Text() { Value = "HELLO TERMINAL", Color = BasicColor.BrightGreen },
        Centered = true
    }
);

game.Run();
