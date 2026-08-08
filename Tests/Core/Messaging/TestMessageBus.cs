using Termule.Engine.Core;
using Termule.Tests.Common;

namespace Termule.Tests.Core.Messaging;

public class TestMessageBus
{
    [Fact]
    public void Broadcast_NotifiesSubscriber()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();
        bus.Subscribe(listener);

        bus.Broadcast(true);

        Assert.True(listener.Message);
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

        Assert.True(listener1.Message);
        Assert.True(listener2.Message);
    }

    [Fact]
    public void Broadcast_NotifiesListenersOfAncestorTypes()
    {
        MessageBus bus = new();
        FakeListener<object> listener = new();
        bus.Subscribe(listener);

        bus.Broadcast(true);

        Assert.Equal(true, listener.Message);
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

    [Fact]
    public void Broadcast_WithoutListeners_DoesNothing()
    {
        MessageBus bus = new();

        bus.Broadcast(true);
    }

    [Fact]
    public void Subscribe_IsIdempotent()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();

        bus.Subscribe(listener);
        bus.Subscribe(listener);

        bus.Broadcast(true);

        Assert.True(listener.Message);
        Assert.Equal(1, listener.MessageCount);
    }

    [Fact]
    public void SubscribeAll_SubscribesForAllTypes()
    {
        MessageBus bus = new();
        MultiTypeListener listener = new();

        bus.SubscribeAll(listener);

        bus.Broadcast(true);
        bus.Broadcast(5);

        Assert.Equal(2, listener.MessagesCount);
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

        Assert.False(listener.Message);
    }

    [Fact]
    public void Unsubscribe_UnsubscribesListener()
    {
        MessageBus bus = new();
        FakeListener<bool> listener = new();
        bus.Subscribe(listener);

        bus.Unsubscribe(listener);

        bus.Broadcast(true);
        Assert.False(listener.Message);
    }

    [Fact]
    public void UnsubscribeAll_UnsubscribesForAllTypes()
    {
        MessageBus bus = new();
        MultiTypeListener listener = new();

        bus.SubscribeAll(listener);
        bus.UnsusbcribeAll(listener);

        bus.Broadcast(true);
        bus.Broadcast(5);

        Assert.Equal(0, listener.MessagesCount);
    }

    private sealed class MultiTypeListener : IMessageListener<bool>, IMessageListener<int>
    {
        public int MessagesCount { get; private set; }

        void IMessageListener<bool>.OnMessage(bool message)
        {
            MessagesCount++;
        }

        void IMessageListener<int>.OnMessage(int message)
        {
            MessagesCount++;
        }
    }
}
