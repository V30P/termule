using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Input;

namespace Termule.Tests.Components;

public class TestHoldControl
{
    [Fact]
    public void Value_IsTrueAsLongAsTheButtonIsDown()
    {
        Game game = new();
        HoldControl holdControl = new(Button.A);
        game.World.Add(holdControl);
        game.Start();

        game.Bus.Broadcast(new ButtonDown(Button.A));
        game.RunTick();

        Assert.True(holdControl.Value);
        game.RunTicks(10);
        Assert.True(holdControl.Value);

        game.Bus.Broadcast(new ButtonUp(Button.A));
        game.RunTick();
        Assert.False(holdControl.Value);
    }
}
