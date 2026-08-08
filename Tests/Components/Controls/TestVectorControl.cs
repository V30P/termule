using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Input;

namespace Termule.Tests.Components;

public class TestVectorControl
{
    public static readonly TheoryData<bool, bool, bool, bool, int, int> ButtonStateData = new()
    {
        { false, false, false, false, 0, 0 },
        { true, false, false, false, 0, 1 },
        { false, true, false, false, -1, 0 },
        { false, false, true, false, 0, -1 },
        { false, false, false, true, 1, 0 },
        { true, true, false, false, -1, 1 },
        { false, true, false, true, 0, 0 },
        { false, true, true, true, 0, -1 },
        { true, true, true, true, 0, 0 },
    };

    [Theory]
    [MemberData(nameof(ButtonStateData))]
    public void Value_ProperlyReflectsButtonState(
        bool upIsPressed,
        bool leftIsPressed,
        bool downIsPressed,
        bool rightIsPressed,
        int expectedValueX,
        int expectedValueY)
    {
        Game game = new();
        VectorControl vectorControl = new(Button.W, Button.A, Button.S, Button.D);
        game.World.Add(vectorControl);
        game.Start();

        if (upIsPressed)
        {
            game.Bus.Broadcast(new ButtonDown(Button.W));
        }
        if (leftIsPressed)
        {
            game.Bus.Broadcast(new ButtonDown(Button.A));
        }
        if (downIsPressed)
        {
            game.Bus.Broadcast(new ButtonDown(Button.S));
        }
        if (rightIsPressed)
        {
            game.Bus.Broadcast(new ButtonDown(Button.D));
        }
        game.RunTick();

        Assert.Equal((expectedValueX, expectedValueY), vectorControl.Value);
    }

    [Fact]
    public void Value_HandlesButtonReleases()
    {
        Game game = new();
        VectorControl vectorControl = new(Button.W, Button.A, Button.S, Button.D);
        game.World.Add(vectorControl);
        game.Start();

        game.Bus.Broadcast(new ButtonDown(Button.W));
        game.RunTicks(10);
        game.Bus.Broadcast(new ButtonUp(Button.W));

        Assert.Equal((0, 0), vectorControl.Value);
    }

    [Fact]
    public void Value_WhenGivenConsecutiveButtonDowns_DoesNotDoubleCount()
    {
        Game game = new();
        VectorControl vectorControl = new(Button.W, Button.A, Button.S, Button.D);
        game.World.Add(vectorControl);
        game.Start();

        game.Bus.Broadcast(new ButtonDown(Button.W));
        game.RunTick();
        game.Bus.Broadcast(new ButtonDown(Button.W));
        game.RunTick();

        Assert.Equal((0, 1), vectorControl.Value);
    }
}
