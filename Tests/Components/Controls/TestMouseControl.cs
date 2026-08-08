using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Input;

namespace Termule.Tests.Components;

public class TestMouseControl
{
    [Fact]
    public void Value_IsLastKnownMousePos()
    {
        Game game = new();
        MouseControl mouseControl = new();
        game.World.Add(mouseControl);
        game.Start();

        game.Bus.Broadcast(new MouseMoved((1, 2)));
        game.RunTick();
        Assert.Equal((1, 2), mouseControl.Value);

        game.Bus.Broadcast(new MouseMoved((3, 4)));
        game.RunTick();
        Assert.Equal((3, 4), mouseControl.Value);
    }
}
