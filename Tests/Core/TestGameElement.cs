using Termule.Engine.Core;
using Termule.Engine.Exceptions;

namespace Termule.Tests.Core;

public class TestGameElement
{
    [Fact]
    public void GetRequiredSystem_ReturnsInstalledSystem()
    {
        IConfigurableGame game = Game.Create();
        FakeGameElement element = new();
        ((Game)game).Register(element);

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, element.CallGetRequiredSystem<FakeSystem>());
    }

    [Fact]
    public void GetRequiredSystem_WhenSystemMissing_Throws()
    {
        IConfigurableGame game = Game.Create();
        FakeGameElement element = new();
        ((Game)game).Register(element);

        Assert.Throws<MissingSystemException<FakeSystem>>(() => element.CallGetRequiredSystem<FakeSystem>());
    }
}