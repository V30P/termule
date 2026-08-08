using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Input;

namespace Termule.Tests.Components;

public class TestComboControl
{
    [Fact]
    public void Value_IsTrueForTheFirstTickAllButtonsAreDown()
    {
        Game game = new();
        ComboControl comboControl = new([Button.A, Button.B]);
        game.World.Add(comboControl);
        game.Start();

        game.Bus.Broadcast(new ButtonDown(Button.A));
        game.RunTick();
        game.Bus.Broadcast(new ButtonDown(Button.B));
        game.RunTick();

        Assert.True(comboControl.Value);
        game.RunTick();
        Assert.False(comboControl.Value);
    }
}
