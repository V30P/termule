namespace Termule.Tests.Core;

using Termule.Core;
using Termule.Tests.Utilities;

public class TestGame
{
    [Fact]
    internal void Register_ShouldConfigureElement()
    {
        Game game = (Game)Game.Create();
        FakeGameElement element = new();

        game.Register(element);
        Assert.Equal(game, element.GameInstance);
        Assert.True(element.RegisteredInvoked);
    }

    [Fact]
    internal void Unregister_ShouldClearElementConfiguration()
    {
        Game game = (Game)Game.Create();
        FakeGameElement element = new();
        game.Register(element);

        game.Unregister(element);

        Assert.Null(element.GameInstance);
        Assert.True(element.UnregisteredInvoked);
    }

    [Fact]
    internal void Create_ShouldInitializeSystemsAndRoot()
    {
        IConfigurableGame game = Game.Create();

        Assert.NotNull(game.Systems);
        Assert.NotNull(game.Root);
    }

    [Fact]
    internal void RunFrame_ShouldTickComponents()
    {
        IConfigurableGame game = Game.Create();
        FakeComponent component = new();
        game.Root.Add(component);
        game.Prepare();

        game.RunForFrames(5);

        Assert.Equal(5, component.TickCount);
    }
}
