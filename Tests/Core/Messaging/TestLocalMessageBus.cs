using Termule.Engine.Core;

namespace Termule.Tests.Core.Messaging;

public class TestLocalMessageBus()
{
    [Fact]
    public void Broadcast_WhenUnregisteredAndRouteIsNonlocal_Throws()
    {
        GameObject gameObject = [];

        Assert.Throws<InvalidOperationException>(() => gameObject.Bus.Broadcast(true, Route.Global));
        Assert.Throws<InvalidOperationException>(() => gameObject.Bus.Broadcast(true, Route.Upwards));
        Assert.Throws<InvalidOperationException>(() => gameObject.Bus.Broadcast(true, Route.Downwards));
    }

    [Fact]
    public void Broadcast_WhenRouteIsLocal_NotifiesListenersOnGameObject()
    {
        GameObject gameObject = [];

        FakeListener<bool> listener = new();
        gameObject.Bus.Subscribe(listener);

        gameObject.Bus.Broadcast(true);

        Assert.True(listener.ReceivedMessage);
    }

    [Fact]
    public void Broadcast_WhenRouteIsGlobal_NotifiesListenersOnTheGame()
    {
        IConfigurableGame game = Game.Create();
        GameObject gameObject = [];
        game.Root.Add(gameObject);

        FakeListener<bool> listener = new();
        game.Bus.Subscribe(listener);

        gameObject.Bus.Broadcast(true, Route.Global);

        Assert.True(listener.ReceivedMessage);
    }

    [Fact]
    public void Broadcast_WhenRouteIsUpwards_NotifiesListenersOnAncestors()
    {
        IConfigurableGame game = Game.Create();
        GameObject gameObject = [];
        GameObject parent = [gameObject];
        game.Root.Add(parent);

        FakeListener<bool> listener = new();
        parent.Bus.Subscribe(listener);

        gameObject.Bus.Broadcast(true, Route.Upwards);

        Assert.True(listener.ReceivedMessage);
    }

    [Fact]
    public void Broadcast_WhenRouteIsDownwards_NotifiesListenersOnDescendants()
    {
        IConfigurableGame game = Game.Create();
        GameObject child = [];
        GameObject gameObject = [child];
        game.Root.Add(gameObject);

        FakeListener<bool> listener = new();
        child.Bus.Subscribe(listener);

        gameObject.Bus.Broadcast(true, Route.Downwards);

        Assert.True(listener.ReceivedMessage);
    }
}