using Termule.Engine.Components;
using Termule.Engine.Core;
using Termule.Engine.Systems.Input;

namespace Termule.Tests.Components;

public class TestCharControl
{
    [Fact]
    public void Value_ContainsAllCharsFromLastTick()
    {
        Game game = new();
        CharControl charControl = new();
        game.World.Add(charControl);
        game.Start();

        game.Bus.Broadcast(new CharTyped('A'));
        game.Bus.Broadcast(new CharTyped('B'));
        game.RunTick();

        Assert.Equal("AB", charControl.Value);
    }

    [Fact]
    public void Value_ClearsBetweenTicks()
    {
        Game game = new();
        CharControl charControl = new();
        game.World.Add(charControl);
        game.Start();

        game.Bus.Broadcast(new CharTyped('A'));
        game.RunTick();
        game.RunTick();

        Assert.Empty(charControl.Value);
    }
}
