using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Systems.Resources;

namespace Termule.Tests.Core;

public class TestSystemManager
{
    [Fact]
    public void Install_AddsSystem()
    {
        IConfigurableGame game = Game.Create();
        FakeSystem system = new();

        game.Systems.Install(system);
        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Install_ReplacesExistingSystem()
    {
        IConfigurableGame game = Game.Create();
        game.Systems.Install(new FakeSystem());

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Install_WhenGameAlreadyStarted_Throws()
    {
        IConfigurableGame game = Game.Create();
        game.Start();

        Assert.Throws<InvalidOperationException>(() => game.Systems.Install(new FakeSystem()));
    }

    [Fact]
    public void Get_WhenSystemMissing_ReturnsNull()
    {
        IConfigurableGame game = Game.Create();

        Assert.Null(game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Uninstall_RemovesSystem()
    {
        IConfigurableGame game = Game.Create();
        game.Systems.Install(new FakeSystem());

        game.Systems.Uninstall<FakeSystem>();

        Assert.Null(game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Uninstall_WhenGameStarted_Throws()
    {
        IConfigurableGame game = Game.Create();
        game.Start();

        Assert.Throws<InvalidOperationException>(() => game.Systems.Uninstall<FakeSystem>());
    }

    [Fact]
    public void InstalledSystem_FollowsLifecycle()
    {
        IConfigurableGame game = Game.Create();
        FakeSystem system = new();
        game.Systems.Install(system);

        game.Start();
        Assert.True(system.Started);

        game.RunForFrames(5);
        Assert.Equal(5, system.TickCount);

        game.CleanUp();
        Assert.True(system.Stopped);
    }

    [Fact]
    public void UseDefaultsInstallsCoreSystems()
    {
        IConfigurableGame game = Game.Create();

        game.Systems.UseDefaults();

        Assert.NotNull(game.Systems.Get<Keyboard>());
        Assert.NotNull(game.Systems.Get<DisplaySystem>());
        Assert.NotNull(game.Systems.Get<RenderSystem>());
        Assert.NotNull(game.Systems.Get<ResourceLoader>());
    }
}