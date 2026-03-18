namespace Termule.Tests.Core;

using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Termule.Core;
using Termule.Tests.Utilities;

public class TestGameElement()
{
    [Fact]
    internal void GetRequiredSystem_ShouldReturnInstalledSystem()
    {
        GetGameWithGameElement(out IConfigurableGame game, out FakeGameElement element);

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, element.CallGetRequiredSystem<FakeSystem>());
    }

    [Fact]
    internal void GetRequiredSystem_ShouldThrow_WhenSystemMissing()
    {
        GetGameWithGameElement(out IConfigurableGame game, out FakeGameElement element);
        Assert.Throws<MissingSystemException<FakeSystem>>(() => element.CallGetRequiredSystem<FakeSystem>());
    }

    private static void GetGameWithGameElement(out IConfigurableGame game, out FakeGameElement element)
    {
        game = Game.Create();
        element = new();
        ((Game)game).Register(element);
    }
}