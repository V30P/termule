using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Input;

namespace Termule.Tests.Components;

public class TestTriggerControl
{
    [Fact]
    public void Value_IsTrueForTheFirstTickTheButtonIsDown()
    {
        Game game = new();
        TriggerControl triggerControl = new(Button.A);
        game.World.Add(triggerControl);
        game.Start();

        game.Bus.Broadcast(new ButtonDown(Button.A));
        game.RunTick();
        Assert.True(triggerControl.Value);
        game.RunTick();
        Assert.False(triggerControl.Value);
    }
}
