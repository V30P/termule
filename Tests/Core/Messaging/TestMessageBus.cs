using Termule.Engine.Core.Messaging;

namespace Termule.Tests.Core.Messaging;

public class TestMessageBus()
{
    [Fact]
    public void Subscribe_IsIdempotent()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();

        bus.Subscribe(listener);
        bus.Subscribe(listener);

        bus.Broadcast(true);

        Assert.True(listener.ReceivedMessage);
        Assert.Equal(1, listener.MessageCount);
    }

    [Fact]
    public void Unsubscribe_IsIdempotent()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();
        bus.Subscribe(listener);

        bus.Unsubscribe(listener);
        bus.Unsubscribe(listener);

        bus.Broadcast(true);

        Assert.False(listener.ReceivedMessage);
    }

    [Fact]
    public void Unsubscribe_UnsubscribesListener()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();
        bus.Subscribe(listener);

        bus.Unsubscribe(listener);

        bus.Broadcast(true);
        Assert.False(listener.ReceivedMessage);
    }

    [Fact]
    public void Broadcast_WithoutListeners_DoesNothing()
    {
        MessageBus bus = new();

        bus.Broadcast(true);
    }

    [Fact]
    public void Broadcast_NotifiesSubscriber()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();
        bus.Subscribe(listener);

        bus.Broadcast(true);

        Assert.True(listener.ReceivedMessage);
    }

    [Fact]
    public void Broadcast_NotifiesMultipleSubscribers()
    {
        MessageBus bus = new();
        FakeListener<bool> listener1 = new();
        FakeListener<bool> listener2 = new();
        bus.Subscribe(listener1);
        bus.Subscribe(listener2);

        bus.Broadcast(true);

        Assert.True(listener1.ReceivedMessage);
        Assert.True(listener2.ReceivedMessage);
    }

    [Fact]
    public void Broadcast_OnlyNotifiesListenersOfCorrectType()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();
        bus.Subscribe(listener);

        bus.Broadcast(1);

        Assert.Equal(0, listener.MessageCount);
    }
}