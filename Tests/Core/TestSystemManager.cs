using Termule.Engine.Core;
using Termule.Engine.Systems.Input;
using Termule.Engine.Systems.Rendering;
using Termule.Engine.Systems.Resources;
using Termule.Tests.Common;

namespace Termule.Tests.Core;

public class TestSystemManager
{
    [Fact]
    public void Get_WhenSystemMissing_ReturnsNull()
    {
        Game game = new();

        Assert.Null(game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Install_AddsSystem()
    {
        Game game = new();
        FakeSystem system = new();

        game.Systems.Install(system);
        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Add_AddsThenActivatesComponentsSimultaneously()
    {
        Game game = new();
        DependentSystem dependentSystem = new();

        game.Systems.Install(dependentSystem, new FakeSystem());

        Assert.True(dependentSystem.HasDependency);
    }

    [Fact]
    public void Install_ReplacesExistingSystem()
    {
        Game game = new();
        game.Systems.Install(new FakeSystem());

        FakeSystem system = new();
        game.Systems.Install(system);

        Assert.Equal(system, game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Install_WhenGameAlreadyStarted_Throws()
    {
        Game game = new();
        game.Start();

        _ = Assert.Throws<InvalidOperationException>(() => game.Systems.Install(new FakeSystem()));
    }

    [Fact]
    public void InstalledSystem_FollowsLifecycle()
    {
        Game game = new();
        FakeSystem system = new();
        game.Systems.Install(system);

        game.Start();
        Assert.True(system.Started);

        game.RunTicks(5);
        Assert.Equal(5, system.TickCount);

        game.CleanUp();
        Assert.True(system.Stopped);
    }

    [Fact]
    public void Uninstall_RemovesSystem()
    {
        Game game = new();
        game.Systems.Install(new FakeSystem());

        game.Systems.Uninstall<FakeSystem>();

        Assert.Null(game.Systems.Get<FakeSystem>());
    }

    [Fact]
    public void Uninstall_WhenGameStarted_Throws()
    {
        Game game = new();
        game.Start();

        _ = Assert.Throws<InvalidOperationException>(game.Systems.Uninstall<FakeSystem>);
    }

    [Fact]
    public void InstallDefaultsInstallsCoreSystems()
    {
        Game game = new();

        game.Systems.InstallDefaults();

        Assert.NotNull(game.Systems.Get<Keyboard>());
        Assert.NotNull(game.Systems.Get<RenderSystem>());
        Assert.NotNull(game.Systems.Get<ResourceLoader>());
    }

    private sealed class DependentSystem : Engine.Core.System
    {
        public bool HasDependency { get; private set; }

        protected override void Activate()
        {
            HasDependency = Systems.Get<FakeSystem>() != null;
        }
    }
}
