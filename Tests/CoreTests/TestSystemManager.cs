using Termule.Core;
using Tests.Utilities;

namespace Tests.CoreTests;

public class TestSystemManager
{
    [Fact]
    internal void Install_ShouldAddSystem()
    {
        var game = Game.Create();
        FakeSystem system = new();

        game.Systems.Install(system);
        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    internal void Install_ShouldReplaceExistingSystem()
    {
        var game = Game.Create();
        game.Systems.Install(new FakeSystem());

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    internal void Install_ShouldThrow_WhenGameStarted()
    {
        var game = Game.Create();
        game.Prepare();

        Assert.Throws<InvalidOperationException>(() => game.Systems.Install(new FakeSystem()));
    }

    [Fact]
    internal void InstalledSystem_ShouldFollowLifecycle()
    {
        var game = Game.Create();
        FakeSystem system = new();
        game.Systems.Install(system);

        game.Prepare();
        Assert.True(system.Started);

        game.RunForFrames(5);
        Assert.Equal(5, system.TickCount);

        game.CleanUp();
        Assert.True(system.Stopped);
    }

    [Fact]
    internal void Uninstall_ShouldRemoveSystem()
    {
        var game = Game.Create();
        game.Systems.Install(new FakeSystem());

        game.Systems.Uninstall<FakeSystem>();

        Assert.Null(game.Systems.Get<FakeSystem>());
    }

    [Fact]
    internal void Uninstall_ShouldThrow_WhenGameStarted()
    {
        var game = Game.Create();
        game.Prepare();

        Assert.Throws<InvalidOperationException>(() => game.Systems.Uninstall<FakeSystem>());
    }
}