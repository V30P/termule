using Termule.Engine.Core;
using Termule.Engine.Exceptions;
using Termule.Tests.Core.Fakes;

namespace Termule.Tests.Core;

public class TestGameElement
{
    private static void GetGameWithGameElement(out IConfigurableGame game, out FakeGameElement element)
    {
        game = Game.Create();
        element = new FakeGameElement();
        ((Game)game).Register(element);
    }

    [Fact]
    public void GetRequiredSystem_ShouldReturnInstalledSystem()
    {
        GetGameWithGameElement(out IConfigurableGame game, out FakeGameElement element);

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, element.CallGetRequiredSystem<FakeSystem>());
    }

    [Fact]
    public void GetRequiredSystem_ShouldThrow_WhenSystemMissing()
    {
        GetGameWithGameElement(out _, out FakeGameElement element);
        Assert.Throws<MissingSystemException<FakeSystem>>(() => element.CallGetRequiredSystem<FakeSystem>());
    }
}