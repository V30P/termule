using Termule.Engine.Core;
using Termule.Tests.Core.Fakes;

namespace Termule.Tests.Core;

public class TestGame
{
    [Fact]
    public void Create_ShouldInitializeSystemsAndRoot()
    {
        var game = Game.Create();

        Assert.NotNull(game.Systems);
        Assert.NotNull(game.Root);
    }

    [Fact]
    public void Register_ShouldConfigureElement()
    {
        var game = (Game)Game.Create();
        FakeGameElement element = new();

        game.Register(element);

        Assert.Equal(game, element.GameInstance);
        Assert.True(element.RegisteredInvoked);
    }

    [Fact]
    public void RunFrame_ShouldTickComponents()
    {
        var game = Game.Create();
        FakeComponent component = new();
        game.Root.Add(component);
        game.Prepare();

        game.RunForFrames(5);

        Assert.Equal(5, component.TickCount);
    }

    [Fact]
    public void Unregister_ShouldClearElementConfiguration()
    {
        var game = (Game)Game.Create();
        FakeGameElement element = new();
        game.Register(element);

        game.Unregister(element);

        Assert.Null(element.GameInstance);
        Assert.True(element.UnregisteredInvoked);
    }
}