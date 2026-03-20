using Termule.Core;
using Termule.Exceptions;
using Tests.Utilities;

namespace Tests.CoreTests;

public class TestGameElement
{
    private static void GetGameWithGameElement(out IConfigurableGame game, out FakeGameElement element)
    {
        game = Game.Create();
        element = new FakeGameElement();
        ((Game)game).Register(element);
    }

    [Fact]
    internal void GetRequiredSystem_ShouldReturnInstalledSystem()
    {
        GetGameWithGameElement(out var game, out var element);

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, element.CallGetRequiredSystem<FakeSystem>());
    }

    [Fact]
    internal void GetRequiredSystem_ShouldThrow_WhenSystemMissing()
    {
        GetGameWithGameElement(out _, out var element);
        Assert.Throws<MissingSystemException<FakeSystem>>(() => element.CallGetRequiredSystem<FakeSystem>());
    }
}