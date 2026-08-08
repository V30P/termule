using Termule.Engine.Core;
using Termule.Tests.Common;

namespace Termule.Tests.Core;

public class TestGame
{
    [Fact]
    public void TargetTicksPerSecond_DefaultsToUnlimited()
    {
        Game game = new();
        Assert.Equal(int.MaxValue, game.TargetTps);
    }

    [Fact]
    public void Run_PreparesAndCleansUp()
    {
        Game game = new();
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
        Game game = new();
        AutoStopSystem system = new();
        game.Systems.Install(system);
        game.Start();

        game.Run();

        Assert.Equal(1, system.StartCount);
        Assert.Equal(1, system.TickCount);
        Assert.Equal(1, system.StopCount);
    }

    [Fact]
    public void RunTick_TicksComponents()
    {
        Game game = new();
        FakeComponent component = new();
        game.World.Add(component);
        game.Start();

        game.RunTicks(5);

        Assert.Equal(5, component.TickCount);
    }

    [Fact]
    public void StartingGame_BroadcastsStartedMessage()
    {
        Game game = new();
        FakeListener<Game.StartedMessage> listener = new();
        game.Bus.Subscribe(listener);

        game.Start();

        Assert.Equal(1, listener.MessageCount);
    }

    [Fact]
    public void CleanUp_IsIdempotent()
    {
        Game game = new();
        CountingSystem system = new();
        game.Systems.Install(system);
        game.Start();

        game.CleanUp();
        game.CleanUp();

        Assert.Equal(1, system.StopCount);
    }

    [Fact]
    public void StoppingGame_BroadcastsStoppedMessage()
    {
        Game game = new();
        FakeListener<Game.StoppedMessage> listener = new();
        game.Bus.Subscribe(listener);
        game.Start();

        game.Stop();

        Assert.Equal(1, listener.MessageCount);
    }

    [Fact]
    public void Activate_BroadcastsElementActivatedMessage()
    {
        Game game = new();
        FakeListener<Game.ElementActivatedMessage> listener = new();
        game.Bus.Subscribe(listener);
        FakeGameElement element = new();

        game.Activate(element);

        Assert.Equal(1, listener.MessageCount);
        Assert.Equal(element, listener.Message.Element);
    }

    [Fact]
    public void Deactivate_BroadcastsElementDeactivatedMessage()
    {
        Game game = new();
        FakeListener<Game.ElementDeactivatedMessage> listener = new();
        game.Bus.Subscribe(listener);

        FakeGameElement element = new();
        game.Activate(element);

        game.Deactivate(element);

        Assert.Equal(1, listener.MessageCount);
        Assert.Equal(element, listener.Message.Element);
    }

    [Fact]
    public void Deactivate_ClearsElementProperties()
    {
        Game game = new();
        FakeGameElement element = new();
        game.Activate(element);

        game.Deactivate(element);

        Assert.Null(element.GameInstance);
        Assert.True(element.HasBeenDeactivated);
    }

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

        protected internal override void CleanUp()
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

        protected internal override void CleanUp()
        {
            StopCount++;
        }
    }
}
