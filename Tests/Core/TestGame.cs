using Termule.Engine.Core;

namespace Termule.Tests.Core;

public class TestGame
{
    private sealed class CountingSystem : Engine.Core.System
    {
        public int StartCount { get; private set; }
        public int TickCount { get; private set; }
        public int StopCount { get; private set; }

        protected internal override void Start()
        {
            StartCount++;
        }

        protected internal override void Tick()
        {
            TickCount++;
        }

        protected internal override void Stop()
        {
            StopCount++;
        }
    }

    private sealed class AutoStopSystem : Engine.Core.System
    {
        public int StartCount { get; private set; }
        public int TickCount { get; private set; }
        public int StopCount { get; private set; }

        protected internal override void Start()
        {
            StartCount++;
        }

        protected internal override void Tick()
        {
            TickCount++;
            Game.Stop();
        }

        protected internal override void Stop()
        {
            StopCount++;
        }
    }

    [Fact]
    public void Create_InitializesSystemsAndRoot()
    {
        IConfigurableGame game = Game.Create();

        Assert.NotNull(game.Systems);
        Assert.NotNull(game.Root);
    }

    [Fact]
    public void Run_PreparesAndCleansUp()
    {
        IConfigurableGame game = Game.Create();
        AutoStopSystem system = new();
        game.Systems.Install(system);

        game.Run();

        Assert.Equal(1, system.StartCount);
        Assert.Equal(1, system.TickCount);
        Assert.Equal(1, system.StopCount);
    }

    [Fact]
    public void Run_WhenAlreadyPrepared_DoesNotPrepareAgain()
    {
        IConfigurableGame game = Game.Create();
        AutoStopSystem system = new();
        game.Systems.Install(system);
        game.Start();

        game.Run();

        Assert.Equal(1, system.StartCount);
        Assert.Equal(1, system.TickCount);
        Assert.Equal(1, system.StopCount);
    }

    [Fact]
    public void RunForFrames_TickSystemsWithoutPreparing()
    {
        IConfigurableGame game = Game.Create();
        CountingSystem system = new();
        game.Systems.Install(system);

        game.RunForFrames(3);

        Assert.Equal(0, system.StartCount);
        Assert.Equal(3, system.TickCount);
        Assert.Equal(0, system.StopCount);
    }

    [Fact]
    public void RunFrame_TicksComponents()
    {
        IConfigurableGame game = Game.Create();
        FakeComponent component = new();
        game.Root.Add(component);
        game.Start();

        game.RunForFrames(5);

        Assert.Equal(5, component.TickCount);
    }

    [Fact]
    public void CleanUp_IsIdempotent()
    {
        IConfigurableGame game = Game.Create();
        CountingSystem system = new();
        game.Systems.Install(system);
        game.Start();

        game.CleanUp();
        game.CleanUp();

        Assert.Equal(1, system.StopCount);
    }

    [Fact]
    public void Register_ConfiguresElement()
    {
        Game game = (Game)Game.Create();
        FakeGameElement element = new();

        game.Register(element);

        Assert.Equal(game, element.GameInstance);
        Assert.True(element.RegisteredInvoked);
    }

    [Fact]
    public void Unregister_ClearsElementProperties()
    {
        Game game = (Game)Game.Create();
        FakeGameElement element = new();
        game.Register(element);

        game.Unregister(element);

        Assert.Null(element.GameInstance);
        Assert.True(element.UnregisteredInvoked);
    }
}