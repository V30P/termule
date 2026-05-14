using Termule.Engine.Core;
using Termule.Engine.Exceptions;

namespace Termule.Tests.Core;

public class TestGameElement
{
    [Fact]
    public void GetRequiredSystem_ReturnsInstalledSystem()
    {
        Game game = new();
        FakeGameElement element = new();
        game.Activate(element);

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, element.CallGetRequiredSystem<FakeSystem>());
    }

    [Fact]
    public void GetRequiredSystem_WhenSystemMissing_Throws()
    {
        Game game = new();
        FakeGameElement element = new();
        game.Activate(element);

        _ = Assert.Throws<MissingSystemException<FakeSystem>>(
            element.CallGetRequiredSystem<FakeSystem>
        );
    }
}
