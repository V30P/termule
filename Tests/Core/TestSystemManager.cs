using Termule.Engine.Core;
using Termule.Engine.Systems.Display;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Systems.Resources;

namespace Termule.Tests.Core;

public class TestSystemManager
{
    [Fact]
    public void Get_ShouldReturnNull_WhenSystemMissing()
    {
        IConfigurableGame game = Game.Create();

        Assert.Null(game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Install_ShouldAddSystem()
    {
        IConfigurableGame game = Game.Create();
        FakeSystem system = new();

        game.Systems.Install(system);
        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Install_ShouldReplaceExistingSystem()
    {
        IConfigurableGame game = Game.Create();
        game.Systems.Install(new FakeSystem());

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Install_ShouldThrow_WhenGameStarted()
    {
        IConfigurableGame game = Game.Create();
        game.Start();

        Assert.Throws<InvalidOperationException>(() => game.Systems.Install(new FakeSystem()));
    }

    [Fact]
    public void InstalledSystem_ShouldFollowLifecycle()
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
    public void Uninstall_ShouldRemoveSystem()
    {
        IConfigurableGame game = Game.Create();
        game.Systems.Install(new FakeSystem());

        game.Systems.Uninstall<FakeSystem>();

        Assert.Null(game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Uninstall_ShouldThrow_WhenGameStarted()
    {
        IConfigurableGame game = Game.Create();
        game.Start();

        Assert.Throws<InvalidOperationException>(() => game.Systems.Uninstall<FakeSystem>());
    }

    [Fact]
    public void UseDefaults_ShouldInstallCoreSystems()
    {
        // Headless test hosts may report invalid console dimensions (-1),
        // which makes terminal display construction fail before defaults install.
        if (Console.WindowWidth <= 0 || Console.WindowHeight <= 0)
        {
            return;
        }

        IConfigurableGame game = Game.Create();

        game.Systems.UseDefaults();

        Assert.NotNull(game.Systems.Get<Keyboard>());
        Assert.NotNull(game.Systems.Get<DisplaySystem>());
        Assert.NotNull(game.Systems.Get<RenderSystem>());
        Assert.NotNull(game.Systems.Get<ResourceLoader>());
    }
}